using DataLayer.Entities.Enums;

namespace Services.Helpers
{
    public static class StringHelper
    {

        public static MediaType GetMediaType(string media)
        {
            if (media.Contains("MEDIA_PHOTO"))
            {
                return MediaType.Photo;
            }

            else if (media.Contains("MEDIA_VIDEO"))
            {
                return MediaType.Video;
            }

            else return MediaType.Unknown;
        }


        public static string GetMediaUrl(string media)
        {
            if (string.IsNullOrEmpty(media)) return media;

            var linkFragment = GetMediaType(media) switch
                {
                    MediaType.Photo => "MEDIA_PHOTO".Length,
                    MediaType.Video => "MEDIA_VIDEO".Length,
                    _ => 0
                };

            return media.Substring(linkFragment + 3);
        }

    }
}
