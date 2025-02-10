using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using sharding.Models;

namespace sharding.service;

public class ConsistentHashing
{
    private int nodesCount = 3;

    public ConsistentHashing(){}

    public int GetNode(string key){
        using HashAlgorithm algorithm = SHA256.Create();
        var bytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(key));
        var numeric = (int)BitConverter.ToInt32(bytes, 0);
        return numeric % nodesCount;
    }
}
