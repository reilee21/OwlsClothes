using Newtonsoft.Json;

namespace Owls.Helper
{
    public class LocationService
    {
        private readonly List<CityData> _cities;

        public LocationService(string jsonData)
        {
            _cities = JsonConvert.DeserializeObject<List<CityData>>(jsonData);
        }

        public string GetCityName(string cityId)
        {
            var city = _cities.FirstOrDefault(c => c.Id == cityId);
            return city?.Name;
        }

        public string GetDistrictName(string cityId, string districtId)
        {
            var city = _cities.FirstOrDefault(c => c.Id == cityId);
            var district = city?.Districts.FirstOrDefault(d => d.Id == districtId);
            return district?.Name;
        }

        public string GetWardName(string cityId, string districtId, string wardId)
        {
            var city = _cities.FirstOrDefault(c => c.Id == cityId);
            var district = city?.Districts.FirstOrDefault(d => d.Id == districtId);
            var ward = district?.Wards.FirstOrDefault(w => w.Id == wardId);
            return ward?.Name;
        }
    }





    public class CityData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DistrictData> Districts { get; set; }
    }

    public class DistrictData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<WardData> Wards { get; set; }
    }

    public class WardData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
