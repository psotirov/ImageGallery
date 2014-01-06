using ImageGalery.Services.Persisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageGallery.Persisters
{
    public static class PubnubPublisher
    {
        public const string PUBLISH_KEY = "pub-c-4bf036f3-f697-47f0-b9c7-b034d49f45d3";
        public const string SUBSCRIBE_KEY = "sub-c-8a8561a8-05ab-11e3-991c-02ee2ddab7fe";
        public const string SECRET_KEY = "sec-c-ZmRiYjkwMmItY2Y1YS00MDgyLWE4MmMtM2Y0NDBmYzQyMDAw";
        public const bool SSL_ON = true;

        private static PubnubAPI pubnub = new PubnubAPI(PUBLISH_KEY, SUBSCRIBE_KEY, SECRET_KEY, SSL_ON);

        public static void Publish(string channel, string message)
        {
            pubnub.Publish(channel, message);
        }
    }
}
