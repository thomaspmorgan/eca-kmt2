namespace ECA.Data
{
    /// <summary>
    /// An IDS2019Fileable entity is an entity that will have a DS2019 file related to it.
    /// </summary>
    public interface IDS2019Fileable
    {
        /// <summary>
        /// Returns the name of the ds 2019 file.
        /// </summary>
        /// <returns></returns>
        string GetDS2019FileName();

        /// <summary>
        /// Gets or sets the ds 2019 file name.
        /// </summary>
        string DS2019FileName { get; set; }
    }
}