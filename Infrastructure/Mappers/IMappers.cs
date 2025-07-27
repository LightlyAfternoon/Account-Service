namespace Account_Service.Infrastructure.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMappers<TDto, TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        static abstract TDto MapToDto(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        static abstract TEntity MapToEntity(TDto dto);
    }
}