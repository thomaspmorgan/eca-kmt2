using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Compression;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace ECA.WebJobs.Sevis.Core.Test
{
    [TestClass]
    public class ZipArchiveDS2019FileProviderTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);
                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive();

                var fileProvider = new ZipArchiveDS2019FileProvider(zipArchive);
                Assert.IsNotNull(fileProvider.ZipArchive);
            }
        }

        [TestMethod]
        public void TestGetStream_ArchiveHasFile()
        {
            using (ShimsContext.Create())
            {
                var memoryStream = new MemoryStream();
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () => memoryStream
                };

                var entries = new List<ZipArchiveEntry>();
                entries.Add(entry);
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var fileProvider = new ZipArchiveDS2019FileProvider(zipArchive);
                Assert.IsTrue(Object.ReferenceEquals(memoryStream, fileProvider.GetStream(sevisId)));
                memoryStream.Dispose();
            }
        }

        [TestMethod]
        public void TestGetStream_ArchiveDoesNotHaveFile()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";

                var entries = new List<ZipArchiveEntry>();
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var fileProvider = new ZipArchiveDS2019FileProvider(zipArchive);
                Assert.IsNull(fileProvider.GetStream(sevisId));
            }
        }

        [TestMethod]
        public async Task TestGetDS2019FileStream_ArchiveHasFile()
        {
            using (ShimsContext.Create())
            {
                var memoryStream = new MemoryStream();
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () => memoryStream
                };

                var entries = new List<ZipArchiveEntry>();
                entries.Add(entry);
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var fileProvider = new ZipArchiveDS2019FileProvider(zipArchive);

                Action<Stream> tester = (testStream) =>
                {
                    Assert.IsTrue(Object.ReferenceEquals(memoryStream, testStream));
                };

                var stream = fileProvider.GetDS2019FileStream(1, "batchId", sevisId);
                var streamAsync = await fileProvider.GetDS2019FileStreamAsync(1, "batchId", sevisId);
                tester(stream);
                tester(streamAsync);

                memoryStream.Dispose();
            }
        }

        [TestMethod]
        public async Task TestGetDS2019FileStream_ArchiveDoesNotHaveFile()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);

                var entries = new List<ZipArchiveEntry>();
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var fileProvider = new ZipArchiveDS2019FileProvider(zipArchive);

                Action<Stream> tester = (testStream) =>
                {
                    Assert.IsNull(testStream);
                };

                var stream = fileProvider.GetDS2019FileStream(1, "batchId", sevisId);
                var streamAsync = await fileProvider.GetDS2019FileStreamAsync(1, "batchId", sevisId);
                tester(stream);
                tester(streamAsync);
            }
        }

        [TestMethod]
        public void TestDispose()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive();

                var fileProvider = new ZipArchiveDS2019FileProvider(zipArchive);
                Assert.IsNotNull(fileProvider.ZipArchive);
                fileProvider.Dispose();
                Assert.IsNull(fileProvider.ZipArchive);
            }
        }
    }
}
