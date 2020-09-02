using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Test_Task.Interfaces;
using Test_Task.Models.Goods;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Xml.Parser;
using System.Text;
using System.IO;
using AngleSharp.Html;

namespace Test_Task.Models
{
    public class Parser
    {
        public Parser()
        {   
        }
        private async Task<string> GetPageSourceAsync(string url)
        {
            using (WebClient wc = new WebClient())
            {
                var request = WebRequest.Create("http://partner.market.yandex.ru/pages/help/YML.xml");
                var response = await request.GetResponseAsync();
                string source = null;
                if (response != null && ((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    using(StreamReader sr = new StreamReader(((HttpWebResponse)response).GetResponseStream()))
                    {
                        source = await sr.ReadToEndAsync();
                    }
                }
                return source;
                
            }
            
            
        }
        private Shop ParseXmlDoc(IDocument doc)
        {
            Shop result = new Shop();
            result.Name = doc.QuerySelector("shop > name").TextContent;
            result.Company = doc.QuerySelector("shop > company").TextContent;
            result.Url = doc.QuerySelector("shop > url").TextContent;
            result.LocalDeliveryCost = Int32.Parse(doc.QuerySelector("shop > local_delivery_cost").TextContent);
            result.Currencies = ParseCurrencies(doc.QuerySelector("currencies"));
            result.Categories = ParseCategories(doc.QuerySelector("categories"));
            result.Offers = ParseOffers(
                doc.QuerySelector("offers"), 
                result.Categories, 
                result.Currencies, 
                result.LocalDeliveryCost);
            return result;
        }
        private List<Currency> ParseCurrencies(IElement parent)
        {
            List<Currency> currencies = new List<Currency>();
            foreach(var currency in parent.QuerySelectorAll("currency"))
            {
                var id = currency.GetAttribute("id");
                var rate = Int32.Parse(currency.GetAttribute("rate"));
                var plus = Int32.Parse(currency.GetAttribute("plus"));
                currencies.Add(new Currency { Id = id, Rate = rate, Plus = plus });
            }
            return currencies;
        }
        private List<Category> ParseCategories(IElement parent)
        {
            List<Category> categories = new List<Category>();
            var categoriesNodes = parent.QuerySelectorAll("category");
            foreach (var cat in categoriesNodes)
            {
                categories.Add(new Category
                {
                    CategoryName = cat.TextContent,
                    Id = Int32.Parse(cat.GetAttribute("id")),
                    ChildCategories = null,
                    ParentCategory = null
                }) ;
            }
            foreach (var cat in categoriesNodes.Where(c=>c.GetAttribute("parentId")!=null))
            {
                categories.First(c => c.Id == Int32.Parse(cat.GetAttribute("id"))).ParentCategory = categories.First(c => c.Id == Int32.Parse(cat.GetAttribute("parentId")));
            }
            foreach (var cat in categories)
            {
                cat.ChildCategories = categories.Where(c => 
                c.ParentCategory!=null && c.ParentCategory.Id == cat.Id).ToList();
            }
            return categories;
        }
        private List<IOffer> ParseOffers(
            IElement parent,
            List<Category> categories,
            List<Currency> currencies,
            int localDeliveryCost)
        {
            List<IOffer> offers = new List<IOffer>();
            IOffer offer;
            foreach(var off in parent.QuerySelectorAll("offer"))
            {
                switch (off.GetAttribute("type"))
                {
                    case "vendor.model":
                        offer = new VendorModel();
                        (offer as VendorModel).TypePrefix = off.QuerySelector("typePrefix").TextContent;
                        (offer as VendorModel).Vendor = off.QuerySelector("vendor").TextContent;
                        (offer as VendorModel).VendorCode = off.QuerySelector("vendorCode").TextContent;
                        (offer as VendorModel).Model = off.QuerySelector("model").TextContent;
                        (offer as VendorModel).ManufacturerWarranty = bool.Parse(off.QuerySelector("manufacturer_warranty").TextContent);
                        (offer as VendorModel).CountryOfOrigin = off.QuerySelector("country_of_origin").TextContent;
                        break;
                    case "book":
                        offer = new Book();
                        (offer as Book).Author = off.QuerySelector("author").TextContent;
                        (offer as Book).Name = off.QuerySelector("name").TextContent;
                        (offer as Book).Publisher = off.QuerySelector("publisher").TextContent;
                        (offer as Book).Series = off.QuerySelector("series").TextContent;
                        (offer as Book).Year = Int32.Parse(off.QuerySelector("year").TextContent);
                        (offer as Book).ISBN = off.QuerySelector("ISBN").TextContent;
                        (offer as Book).Volume = Int32.Parse(off.QuerySelector("volume").TextContent);
                        (offer as Book).Part = Int32.Parse(off.QuerySelector("part").TextContent);
                        (offer as Book).Language = off.QuerySelector("language").TextContent;
                        (offer as Book).Binding = off.QuerySelector("binding").TextContent;
                        (offer as Book).PageExtent = Int32.Parse(off.QuerySelector("page_extent").TextContent);
                        (offer as Book).Delivery = bool.Parse(off.QuerySelector("delivery").TextContent);
                        (offer as Book).LocalDeliveryCost = Int32.Parse(off.QuerySelector("local_delivery_cost")?.TextContent);
                        break;
                    case "audiobook":
                        offer = new Audiobook();
                        (offer as Audiobook).Author = off.QuerySelector("author").TextContent;
                        (offer as Audiobook).Name = off.QuerySelector("name").TextContent;
                        (offer as Audiobook).Publisher = off.QuerySelector("publisher").TextContent;
                        (offer as Audiobook).Year = Int32.Parse(off.QuerySelector("year").TextContent);
                        (offer as Audiobook).ISBN = off.QuerySelector("ISBN").TextContent;
                        (offer as Audiobook).Language = off.QuerySelector("language").TextContent;
                        (offer as Audiobook).PerformedBy = off.QuerySelector("performed_by").TextContent;
                        (offer as Audiobook).PerformanceType = off.QuerySelector("performance_type").TextContent;
                        (offer as Audiobook).Storage = off.QuerySelector("storage").TextContent;
                        (offer as Audiobook).Format = off.QuerySelector("format").TextContent;
                        //  Formats for parse
                        string[] formats= new string[]{ "h'h'm'm's's'","m'm's's'"};
                        (offer as Audiobook).RecordingLength = TimeSpan.ParseExact(off.QuerySelector("recording_length").TextContent, formats, CultureInfo.InvariantCulture);
                        break;
                    case "artist.title":
                        offer = new ArtistTitle();
                        (offer as ArtistTitle).Title = off.QuerySelector("title").TextContent;
                        (offer as ArtistTitle).Year = Int32.Parse(off.QuerySelector("year").TextContent);
                        (offer as ArtistTitle).Media = off.QuerySelector("media").TextContent;
                        (offer as ArtistTitle).Artist = off.QuerySelector("artist")?.TextContent;
                        (offer as ArtistTitle).Starring = off.QuerySelector("starring")?.TextContent?.Split(',');
                        (offer as ArtistTitle).Director = off.QuerySelector("director")?.TextContent;
                        (offer as ArtistTitle).OriginalName = off.QuerySelector("originalName")?.TextContent;
                        (offer as ArtistTitle).Country = off.QuerySelector("country")?.TextContent;
                        break;
                    case "tour":
                        offer = new Tour();
                        (offer as Tour).WorldRegion = off.QuerySelector("worldRegion").TextContent;
                        (offer as Tour).Country = off.QuerySelector("country").TextContent;
                        (offer as Tour).Region = off.QuerySelector("region").TextContent;
                        (offer as Tour).Days = Int32.Parse(off.QuerySelector("days").TextContent);
                        (offer as Tour).DataTourStart = DateTime.Parse(off.QuerySelectorAll("dataTour")[0].TextContent,CultureInfo.InvariantCulture);
                        (offer as Tour).DataTourEnd = DateTime.Parse(off.QuerySelectorAll("dataTour")[1].TextContent,CultureInfo.InvariantCulture);
                        (offer as Tour).Name = off.QuerySelector("name").TextContent;
                        (offer as Tour).HotelStars = Int32.Parse(off.QuerySelector("hotel_stars").TextContent.Replace("*",string.Empty));
                        (offer as Tour).Room = off.QuerySelector("room").TextContent;
                        (offer as Tour).Meal = off.QuerySelector("meal").TextContent;
                        (offer as Tour).Included= off.QuerySelector("included").TextContent.Split(',');
                        (offer as Tour).Transport = off.QuerySelector("transport").TextContent;
                        break;
                    case "event-ticket":
                        offer = new EventTicket();
                        (offer as EventTicket).Name = off.QuerySelector("name").TextContent;
                        (offer as EventTicket).Place = off.QuerySelector("place").TextContent;
                        (offer as EventTicket).Hall = off.QuerySelector("hall").TextContent;
                        (offer as EventTicket).HallPart = off.QuerySelector("hall_part").TextContent;
                        (offer as EventTicket).Date = DateTime.Parse(off.QuerySelector("date").TextContent, CultureInfo.InvariantCulture);
                        (offer as EventTicket).IsPremiere = Convert.ToBoolean(Int32.Parse(off.QuerySelector("is_premiere").TextContent));
                        (offer as EventTicket).IsPremiere = Convert.ToBoolean(Int32.Parse(off.QuerySelector("is_kids").TextContent));
                        break;
                    default:
                        offer = new BaseOffer();
                        break;
                }
                ParseBaseOffer(off,offer,categories,currencies,localDeliveryCost);
                offers.Add(offer);
            }
            return offers;
        }
        private void ParseBaseOffer(
            IElement offerNode,
            IOffer offer,
            List<Category> categories,
            List<Currency> currencies,
            int localDeliveryCost)
        {
            offer.Id = Int32.Parse(offerNode.GetAttribute("id"));
            offer.BId = Int32.Parse(offerNode.GetAttribute("bid"));
            if(offerNode.GetAttribute("cbid")!=null)
            {
                offer.CbId = Int32.Parse(offerNode.GetAttribute("cbid"));
            }
            offer.Available = bool.Parse(offerNode.GetAttribute("available"));
            offer.Url = offerNode.QuerySelector("url").TextContent;
            offer.Price = Int32.Parse(offerNode.QuerySelector("price").TextContent);
            offer.Currency = currencies.First(cur => cur.Id == offerNode.QuerySelector("currencyId").TextContent);
            offer.Category = categories.First(cat => cat.Id == Int32.Parse(offerNode.QuerySelector("categoryId").TextContent));
            offer.Picture = offerNode.QuerySelector("picture").TextContent;
            offer.Description = offerNode.QuerySelector("description").TextContent;
            if(offer is IDeliveryOffer)
            {
                (offer as IDeliveryOffer).Delivery = bool.Parse(offerNode.QuerySelector("delivery").TextContent);
                if(offerNode.QuerySelector("local_delivery_cost")!=null)
                {
                    (offer as IDeliveryOffer).LocalDeliveryCost = Int32.Parse(offerNode.QuerySelector("local_delivery_cost").TextContent);
                }
                else
                {
                    (offer as IDeliveryOffer).LocalDeliveryCost = localDeliveryCost;
                }
            }
            if(offer is IDownloadableOffer)
            {
                (offer as IDownloadableOffer).Downloadable = bool.Parse(offerNode.QuerySelector("downloadable").TextContent);
            }

        }
        public async Task<Shop> GetShopFromUrl(string url)
        {
            var source = await GetPageSourceAsync(url);
            XmlParser xmlParser = new XmlParser();
            var xmlDoc  = await xmlParser.ParseDocumentAsync(source);
            return ParseXmlDoc(xmlDoc);
        }   
    }
}
