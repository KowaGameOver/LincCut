﻿using LincCut.Models;
using LincCut.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Text;

namespace LincCut.ServiceLayer
{
    public interface IService
    {
        Task<UrlInfo> OkAddUrlAsync(IUrlInfoRepository repository, string url, IClickRepository repositoryForClicks);
        Task<string> OkRedirectResult(IUrlInfoRepository repository, string url, [Optional]int counter);
    }
}