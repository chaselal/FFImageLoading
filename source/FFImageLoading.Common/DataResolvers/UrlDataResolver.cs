﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading.Cache;
using FFImageLoading.Config;
using FFImageLoading.Work;

namespace FFImageLoading.DataResolvers
{
    public class UrlDataResolver : IDataResolver
    {
        public UrlDataResolver(Configuration configuration)
        {
            Configuration = configuration;
        }

        protected IDownloadCache DownloadCache { get { return Configuration.DownloadCache; } }
        protected Configuration Configuration { get; private set; }

        public async virtual Task<Tuple<Stream, LoadingResult, ImageInformation>> Resolve(string identifier, TaskParameter parameters, CancellationToken token)
        {

            var downloadedData = await DownloadCache.DownloadAndCacheIfNeededAsync(identifier, parameters, Configuration, token).ConfigureAwait(false);

            if (token.IsCancellationRequested)
            {
                downloadedData?.ImageStream.TryDispose();
                token.ThrowIfCancellationRequested();
            }

            var imageInformation = new ImageInformation();
            imageInformation.SetPath(identifier);
            imageInformation.SetFilePath(downloadedData?.FilePath);

            return new Tuple<Stream, LoadingResult, ImageInformation>(
                downloadedData?.ImageStream, downloadedData.RetrievedFromDiskCache ? LoadingResult.DiskCache : LoadingResult.Internet, imageInformation);
        }
    }
}
