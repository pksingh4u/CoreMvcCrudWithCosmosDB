﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMvcCrudWithCosmosDB.Models;
using CoreMvcCrudWithCosmosDB.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace CoreMvcCrudWithCosmosDB.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }


        public async Task AddStudentAsync(Student student)
        {
            await this._container.CreateItemAsync<Student>(student, new PartitionKey(student.Id));
        }

        public async Task DeleteStudentAsync(string id)
        {
            await this._container.DeleteItemAsync<Student>(id, new PartitionKey(id));
        }

        public async Task<Student> GetStudentAsync(string id)
        {
            try
            {
                ItemResponse<Student> response = await this._container.ReadItemAsync<Student>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(string Student)
        {
            var query = this._container.GetItemQueryIterator<Student>(new QueryDefinition(Student));
            List<Student> results = new List<Student>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateStudentAsync(string id, Student student)
        {
            await this._container.UpsertItemAsync<Student>(student, new PartitionKey(id));
        }
    }
}
