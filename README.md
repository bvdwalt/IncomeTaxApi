# IncomeTaxApi
Azure Fuction API for calculating South African income tax for individuals

![.NET Build & Release](https://github.com/bvdwalt/IncomeTaxApi/actions/workflows/dotnet.yml/badge.svg)

Supported Tax Years: 
- 2022
- 2021

Features two endpoints:
- IncomeTaxPerAnnum
  - Query Parameters:
    - GrossIncome (double) - Your per annum income before tax gets deducted.
    - TaxYear (int) - The Tax Year this income is for.
    - Age (int) - Your Age during this tax year
    - SpecificProperty (string) - If you only want a single property to be returned, e.g. IncomeAfterTax
- IncomeTaxPerMonth
  - Query Parameters:
    - GrossIncome (double) - Your per month income before tax gets deducted.
    - TaxYear (int) - The Tax Year this income is for.
    - Age (int) - Your Age during this tax year
    - SpecificProperty (string) - If you only want a single property to be returned, e.g. IncomeAfterTax
