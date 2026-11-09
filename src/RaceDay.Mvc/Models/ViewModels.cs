using System.ComponentModel.DataAnnotations;
namespace RaceDay.Mvc.Models;
public class LoginVm { [Required,EmailAddress] public string Email {get;set;}=""; [Required,DataType(DataType.Password)] public string Password {get;set;}=""; }
public class RegisterVm { [Required] public string FirstName {get;set;}=""; [Required] public string LastName {get;set;}=""; [Required,EmailAddress] public string Email {get;set;}=""; [Required,MinLength(8)] public string Password {get;set;}=""; [Required] public string Role {get;set;}="Participant"; }
public class EventVm { public int EventId {get;set;} [Required] public string Title {get;set;}=""; public string Description {get;set;}=""; [Required] public string Location {get;set;}=""; [Required] public DateTime EventDate {get;set;}=DateTime.Today.AddDays(30); [Required] public string EventType {get;set;}="Run"; public string? BannerUrl {get;set;} }
public class CategoryVm { public int CategoryId {get;set;} public int EventId {get;set;} [Required] public string CategoryName {get;set;}=""; [Range(0.1,1000)] public decimal DistanceKm {get;set;}=5; [Range(0,100000)] public decimal EntryFee {get;set;} public string? AgeGroup {get;set;} }
public class EnrolVm { public int CategoryId {get;set;} [Required] public string EmergencyContact {get;set;}=""; }
public class ProfileVm { [Required] public string FirstName {get;set;}=""; [Required] public string LastName {get;set;}=""; public string? Phone {get;set;} public string? Email {get;set;} public string? Role {get;set;} public string? ProfileImageUrl {get;set;} }
