namespace Summervik.Validators.Tests;

public class SocialSecurityNumberTests
{
    [Theory]
    [InlineData("123-12-1233")]
    [InlineData("123121233")]
    public void Ssn_ValidStructure(string ssn)
    {
        Assert.True(SocialSecurityNumber.IsValidStructure(ssn));
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("11")]
    [InlineData("111")]
    [InlineData("111-1")]
    [InlineData("111-11")]
    [InlineData("111-11-1")]
    [InlineData("111-11-11")]
    [InlineData("111-11-111")]
    [InlineData("111-11-11111")]
    [InlineData("111--1")]
    [InlineData("111--11")]
    [InlineData("111--11-1")]
    [InlineData("111--11-11")]
    [InlineData("111--11-111")]
    [InlineData("111--11-11111")]
    [InlineData("111-1-")]
    [InlineData("111-11--")]
    [InlineData("111-11--1")]
    [InlineData("111-11--11")]
    [InlineData("111-11--111")]
    [InlineData("111-11--11111")]
    [InlineData("111-11--1111-")]
    [InlineData("-111-11--1111-")]
    public void Ssn_InvalidStructure(string ssn)
    {
        Assert.False(SocialSecurityNumber.IsValidStructure(ssn));
    }

    [Theory]
    [InlineData("001-01-0001")] // Low area, non-zero group/serial
    [InlineData("236-01-0001")]
    [InlineData("247-01-0001")]
    [InlineData("586-01-0001")]
    [InlineData("700-01-0001")]
    [InlineData("749-01-0001")]
    [InlineData("123-45-6789")] // Standard valid
    [InlineData("889-12-3456")] // High but valid post-randomization
    public void Ssn_Valid(string ssn)
    {
        Assert.True(SocialSecurityNumber.IsValid(ssn));
    }

    [Theory]
    [InlineData("000-00-0000")]
    [InlineData("000-11-1111")]
    [InlineData("111-00-1111")]
    [InlineData("111-11-0000")]
    [InlineData("666-11-1111")]
    [InlineData("900-11-1111")]
    [InlineData("999-11-1111")]
    public void Ssn_Invalid(string ssn)
    {
        Assert.False(SocialSecurityNumber.IsValid(ssn));
    }
}
