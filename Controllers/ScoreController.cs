using Microsoft.AspNetCore.Mvc;
using ScoreHubAPI.Service;
using ScoreHubAPI.Entities.Dto;
using ScoreHubAPI.Rules;
using ScoreHubAPI.Entities.Extensions;

namespace ScoreHubAPI.Controllers;

[ApiController]
[Route("/scores")]
public class ScoreController(ScoreService scoreService, ScoreRules scoreRules) : ControllerBase
{

    [HttpGet("data")]
    public async Task<IActionResult> GetAllDataAsync()
    {
        var scores = await scoreService.GetAllDataAsync();
        return Ok(scores);
    }

    [HttpGet("{name}/data")]
    public async Task<IActionResult> GetDataByNameAsync(string name)
    {
        var score = await scoreService.GetDataByNameAsync(name);
        return Ok(score);
    }

    [HttpGet("{name}")]
    public IActionResult GetFileByNameAsync(string name)
    {
        var score = scoreService.GetFileByNameAsync(name);
        return new FileStreamResult(score, "application/vnd.recordare.musicxml+xml");
    }

    [HttpPost("data")]
    public async Task<IActionResult> SaveDataAsync([FromForm] ScoreDto dto)
    {
        var score = dto.AsScore();
        await scoreRules.HandleSaveAsync(score);
        await scoreService.SaveDataAsync(score);
        return Ok();
    }

    [HttpPost("chunks")]
    public async Task<IActionResult> SaveFileAsync([FromForm] ChunkDto dto)
    {
        await scoreService.SaveFileAsync(dto);
        return Ok();
    }

    [HttpPost("json")]
    public async Task<IActionResult> SaveJsonAsync(ScoreDto dto)
    {
        var score = dto.AsScore();
        await scoreRules.HandleSaveAsync(score);
        if (dto.Content == null) return BadRequest("Must provide content.");
        await scoreService.SaveJsonAsync(score, dto.Content);
        return Ok();
    }

    [HttpPatch("data")]
    public async Task<IActionResult> UpdateDataAsync([FromForm] MusicEditDto dto)
    {
        scoreRules.HandleUpdateData(dto);
        await scoreService.UpdateDataAsync(dto);
        return Ok();
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAsync(string name)
    {
        await scoreService.DeleteAsync(name);
        return Ok();
    }
}