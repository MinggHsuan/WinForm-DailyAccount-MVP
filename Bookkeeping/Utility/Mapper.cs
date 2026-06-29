using AutoMapper;
using Bookkeeping.Models;
using Bookkeeping.Models.DTOs;
using CSVlibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Utility
{
    public static class Mapper
    {
        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> record, Action<IMappingExpression<TSource, TDestination>> action)
        {
            var config = new MapperConfiguration(x =>
            {
                var mappingExpression = x.CreateMap<TSource, TDestination>();
                action.Invoke(mappingExpression);
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(record);
        }

        public static TDestination Map<TSource, TDestination>(TSource record, Action<IMappingExpression<TSource, TDestination>> action)
        {
            var config = new MapperConfiguration(x =>
            {
                var mappingExpression = x.CreateMap<TSource, TDestination>();
                action.Invoke(mappingExpression);
            });
            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(record);
        }

        public static IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> record)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(record);
        }

        public static TDestination Map<TSource, TDestination>(TSource record)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(record);
        }
    }
}
