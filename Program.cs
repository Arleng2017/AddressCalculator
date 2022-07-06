using System;
using System.Collections.Generic;
using System.Linq;

namespace AddressCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string addrss = "บริษัท เซ็นทรัล ฟู้ด รีเทล สาขา FDC เลขที่ 8/5-7 หมู่5";
            var address = AddressCalCulator($"{addrss} {null}");
            var build = address.BuildingName;
        }


        private static dynamic AddressCalCulator(string address)
        {
            address = address.Replace("เลขที่", " ");
            var addressArray = address.Split(' ').ToList();
            var houseNumber = "";
            bool isHasHouseNumber = addressArray.Any(x => x.Any(char.IsDigit));
            if (isHasHouseNumber)
            {
                var isHouseNumber = addressArray.Where(x => x.Any(char.IsDigit))
                                                .FirstOrDefault()
                                                .Replace("/", "")
                                                .Replace("(", "")
                                                .Replace(")", "")
                                                 .Replace("-", "")
                                                .All(char.IsDigit);
                if (isHouseNumber)
                {
                    houseNumber = addressArray.Where(x => x.Any(char.IsDigit)).FirstOrDefault();
                    int houseNumberIndex = GetAddressIndex(addressArray, houseNumber);
                    houseNumber = addressArray[houseNumberIndex];
                    addressArray.RemoveAt(houseNumberIndex);
                }
            }

            string street = "";
            bool isHasStreet = IsHasInfoHasInfo(addressArray, "ถ.");
            if (isHasStreet)
            {
                int streetIndex = GetAddressIndex(addressArray, "ถ.");
                street = addressArray[streetIndex].Replace("ถ.", "");
                var isStreetHasNumber = IsHasInfoHasNumber(addressArray, streetIndex);
                if (isStreetHasNumber)
                {
                    street = $"{street} {addressArray[streetIndex + 1]}";
                    addressArray.RemoveAt(streetIndex + 1);
                }
                addressArray.RemoveAt(streetIndex);
            }

            string villageName = "";
            bool isHasVillageName = IsHasInfoHasInfo(addressArray, "หมู่บ้าน");
            if (isHasVillageName)
            {
                int villageNameIndex = GetAddressIndex(addressArray, "หมู่บ้าน");
                villageName = addressArray[villageNameIndex];
                var isvillageNameHasNumber = IsHasInfoHasNumber(addressArray, villageNameIndex);
                if (isvillageNameHasNumber)
                {
                    villageName = $"{villageName} {addressArray[villageNameIndex + 1]}";
                    addressArray.RemoveAt(villageNameIndex + 1);
                }
                addressArray.RemoveAt(villageNameIndex);
            }

            string buildingName = "";
            bool isHasBuildingName = IsHasInfoHasInfo(addressArray, "อาคาร");
            if (isHasBuildingName)
            {
                int buildingNameIndex = GetAddressIndex(addressArray, "อาคาร");
                villageName = addressArray[buildingNameIndex];
                var isvillageNameHasNumber = IsHasInfoHasNumber(addressArray, buildingNameIndex);
                if (isvillageNameHasNumber)
                {
                    villageName = $"{villageName} {addressArray[buildingNameIndex + 1]}";
                    addressArray.RemoveAt(buildingNameIndex + 1);
                }
                addressArray.RemoveAt(buildingNameIndex);
            }

            string moo = "";
            bool isHasMoo = IsHasInfoHasInfo(addressArray, "หมู่", "หมู่บ้าน");
            if (isHasMoo)
            {
                int mooIndex = GetAddressIndex(addressArray, "หมู่", "หมู่บ้าน");
                moo = addressArray[mooIndex].Replace("หมู่", "").Replace("ที่", "");
                bool isMooContainDigit = string.IsNullOrWhiteSpace(moo);
                if (isMooContainDigit)
                {
                    moo = addressArray[mooIndex + 1];
                    addressArray.RemoveAt(mooIndex + 1);
                }
                addressArray.RemoveAt(mooIndex);
            }

            string soi = "";
            bool isHasSoi = IsHasInfoHasInfo(addressArray, "ซ.");
            if (isHasSoi)
            {
                int soiIndex = GetAddressIndex(addressArray, "ซ.");
                soi = addressArray[soiIndex].Substring(2);
                var isSoiHasNumber = IsHasInfoHasNumber(addressArray, soiIndex);
                if (isSoiHasNumber)
                {
                    soi = $"{soi} {addressArray[soiIndex + 1]}";
                    addressArray.RemoveAt(soiIndex);
                }
                addressArray.RemoveAt(soiIndex);
            }

            isHasSoi = IsHasInfoHasInfo(addressArray, "ตรอก");
            if (isHasSoi)
            {
                int soiIndex = GetAddressIndex(addressArray, "ตรอก");
                soi = addressArray[soiIndex].Replace("ตรอก", "");
                var isSoiHasNumber = IsHasInfoHasNumber(addressArray, soiIndex);
                if (isSoiHasNumber)
                {
                    soi = $"{soi} {addressArray[soiIndex + 1]}";
                    addressArray.RemoveAt(soiIndex);
                }
                addressArray.RemoveAt(soiIndex);
            }

            string yak = "";
            bool isHasYak = IsHasInfoHasInfo(addressArray, "แยก");
            if (isHasYak)
            {
                int yakIndex = GetAddressIndex(addressArray, "แยก");
                yak = addressArray[yakIndex];
                var isSoiHasNumber = IsHasInfoHasNumber(addressArray, yakIndex);
                if (isSoiHasNumber)
                {
                    yak = $"{yak} {addressArray[yakIndex + 1]}";
                    addressArray.RemoveAt(yakIndex);
                }
                addressArray.RemoveAt(yakIndex);
            }

            string buildingInfo = string.Join(" ", addressArray.ToArray());
            var addrss = new
            {
                houseNumber = houseNumber,
                street = street,
                BuildingName = $"{villageName} {buildingName} {buildingInfo }",
                moo = moo,
                soi = !string.IsNullOrWhiteSpace(yak) ? $"{soi} {yak}" : soi
            };
            return addrss;
        }


        public static int GetAddressIndex(List<string> addressArray, string subTitle, string notSubTitle = null)
        {
            int index;
            if (!string.IsNullOrEmpty(notSubTitle))
            {
                string info = addressArray.Where(x => x.Contains(subTitle) && !x.Contains(notSubTitle)).FirstOrDefault();
                index = addressArray.IndexOf(info);
            }
            else
            {
                string info = addressArray.Where(x => x.Contains(subTitle)).FirstOrDefault();
                index = addressArray.IndexOf(info);
            }
            return index;
        }

        public static bool IsHasInfoHasInfo(List<string> addressArray, string subTitle, string notSubTitle = null)
        {
            bool isHasInfoHasInfo;
            if (!string.IsNullOrEmpty(notSubTitle))
            {
                isHasInfoHasInfo = addressArray.Any(x => x.Contains(subTitle) && !x.Contains(notSubTitle));
            }
            else
            {
                isHasInfoHasInfo = addressArray.Any(x => x.Contains(subTitle));
            }
            return isHasInfoHasInfo;
        }

        public static bool IsHasInfoHasNumber(List<string> addressArray, int infoIndex)
        {
            if (infoIndex + 1 < addressArray.Count())
            {
                return addressArray[infoIndex + 1].All(char.IsDigit);
            }
            else
            {
                return false;
            }
        }


    }
}
