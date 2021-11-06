using DataLayer.Entities;
using DataLayer.Repositories;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.DatabaseParser
{

    public interface IDatabaseSeederService
    {
        Task GetContentAsync(Type entityType, string filename);
    }

    public class DatabaseSeederService : IDatabaseSeederService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly string _path;

        public DatabaseSeederService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _path = Environment.CurrentDirectory + "\\hootsuiteDb";
        }

        public async Task GetContentAsync(Type entityType, string filename)
        {
            var text = (await File.ReadAllLinesAsync($@"{_path}\\{filename}")).ToList()
                .Skip(1).ToList();

            if (entityType == typeof(FacebookPost))
            {
                Console.WriteLine("Unpacking facebook-db tsv...");
                var result = await Facebook_DataseedAsync(text.ToList());
                Console.WriteLine(result ? "Done!" : "Something went wrong");
            }
        }

        private async Task<bool> Facebook_DataseedAsync(List<string> dataset)
        {
            var facebookPosts = new List<FacebookPost>();
            foreach (var line in dataset)
            {
                var contents = line.Split("\t");
                var postDate = DateTimeHelper.GetUtcFromEpoch(int.Parse(contents[1]));
                var mediaLink = StringHelper.GetMediaUrl(contents[3]);
                var mediaType = StringHelper.GetMediaType(contents[3]);

                var post = new FacebookPost
                {
                    Date       = postDate,
                    Content    = contents[2],
                    MediaFile  = mediaLink,
                    MediaType  = mediaType,
                    Url        = contents[4],
                    Comments   = int.Parse(contents[5]),
                    Likes      = int.Parse(contents[6]),
                    Shares     = int.Parse(contents[7]),
                    Reactions  = int.Parse(contents[8]),
                    PostNumber = int.Parse(contents[0])
                };

                facebookPosts.Add(post);
            }
            _unitOfWork.FacebookPosts.InsertRange(facebookPosts);
            return await _unitOfWork.SaveChangesAsync();
        }

    }
}
