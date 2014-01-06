using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Spring.Social.Dropbox.Connect;
using Spring.Social.Dropbox.Api;
using System.IO;
using Spring.Social.OAuth1;
using Spring.IO;

namespace ImageGalery.Services.Persisters
{
    public static class DropBoxUploader
    {
        private const string DropboxAppKey = "91uzerc6j1loygn";
        private const string DropboxAppSecret = "ool3f8cs4e5lhsk";
        private const string OAuthTokenFileName = "OAuthTokenFileName.txt"; //Not Used


        public static string UploadProfilePicToDropBox(string filePath, string fileName)
        {
            DropboxServiceProvider dropboxServiceProvider =
                new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);
            
            // Authenticate the application (if not authenticated) and load the OAuth token
            //if (!File.Exists(OAuthTokenFileName))
            //{
            //ImageGallery.Persisters.DropboxManager.AuthorizeAppOAuth(dropboxServiceProvider);
            //}

            OAuthToken oauthAccessToken = new OAuthToken("1ni4swcml3tpqmyo", "7v3zjtj3e42f9bt");

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            //// Display user name (from his profile)
            //DropboxProfile profile = dropbox.GetUserProfileAsync().Result;
            //Console.WriteLine("Hi " + profile.DisplayName + "!");

            // Create new folder
            string newFolderName = DateTime.Now.Ticks.ToString();
            //Entry createFolderEntry = dropbox.CreateFolderAsync(newFolderName).Result;
            //Console.WriteLine("Created folder: {0}", createFolderEntry.Path);

            // Upload a file
            Entry uploadFileEntry = dropbox.UploadFileAsync(
                new FileResource(filePath),
                "/" + newFolderName + "_" + fileName.Substring(1, fileName.Length - 2)).Result;
           

            // Share a file
            DropboxLink sharedUrl = dropbox.GetMediaLinkAsync(uploadFileEntry.Path).Result;
           
           
            string urlToDropbox = sharedUrl.Url.TrimEnd(new char[] { '_', ' ' });
            return urlToDropbox;
        }
    }
}