using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumMStestProject.Queries
{
    internal class GetAddress : ISqlQuery
    {
        public string Query => "SELECT * FROM Address WHERE AddressId = @AddressId";
        public Dictionary<string, object> Parameters { get; set; }

        public IEnumerable<ISqlQueryMapping> EntityTypeMappings => throw new NotImplementedException();

        public IEnumerable<ISqlQueryColumn> Columns => throw new NotImplementedException();

        public string Sql => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string? Schema => throw new NotImplementedException();

        public IRelationalModel Model => throw new NotImplementedException();

        public bool IsShared => throw new NotImplementedException();

        IEnumerable<ITableMappingBase> ITableBase.EntityTypeMappings => EntityTypeMappings;

        public IEnumerable<ITableMappingBase> ComplexTypeMappings => throw new NotImplementedException();

        IEnumerable<IColumnBase> ITableBase.Columns => Columns;

        public object? this[string name] => throw new NotImplementedException();

        public GetAddress(int addressId)
        {
            Parameters = new Dictionary<string, object>
            {
                { "@AddressId", addressId }
            };
        }

        public ISqlQueryColumn? FindColumn(string name)
        {
            throw new NotImplementedException();
        }

        public ISqlQueryColumn? FindColumn(IProperty property)
        {
            throw new NotImplementedException();
        }

        IColumnBase? ITableBase.FindColumn(string name)
        {
            return FindColumn(name);
        }

        IColumnBase? ITableBase.FindColumn(IProperty property)
        {
            return FindColumn(property);
        }

        public IEnumerable<IForeignKey> GetRowInternalForeignKeys(IEntityType entityType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IForeignKey> GetReferencingRowInternalForeignKeys(IEntityType entityType)
        {
            throw new NotImplementedException();
        }

        public bool IsOptional(ITypeBase typeBase)
        {
            throw new NotImplementedException();
        }

        public IAnnotation? FindRuntimeAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAnnotation> GetRuntimeAnnotations()
        {
            throw new NotImplementedException();
        }

        public IAnnotation AddRuntimeAnnotation(string name, object? value)
        {
            throw new NotImplementedException();
        }

        public IAnnotation SetRuntimeAnnotation(string name, object? value)
        {
            throw new NotImplementedException();
        }

        public IAnnotation? RemoveRuntimeAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        public TValue GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory, TArg? factoryArgument)
        {
            throw new NotImplementedException();
        }

        public IAnnotation? FindAnnotation(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAnnotation> GetAnnotations()
        {
            throw new NotImplementedException();
        }
    }
}