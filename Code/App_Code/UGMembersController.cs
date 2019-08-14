/**
 * Author r.tarik
 * Added on 24.05.2019
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Web.WebApi;
using Umbraco.Core.Services;
using Umbraco.Core.Models;
using System.Web.Http;

namespace UGardian.Api
{
    public class UGMembersController : UmbracoAuthorizedApiController
    {
        //ServiceContext _serviceContext = new Services.MemberService();

        /// <summary>
        /// Gets a list of all Umbraco members
        /// </summary>
        /// <returns>A list of members</returns>
        public IEnumerable<IMember> GetAllMembers()
        {
            var memberService = Services.MemberService;
            return memberService.GetAllMembers();
        }

        /// <summary>
        /// Gets a list of all Umbraco members
        /// </summary>
        /// <returns>A list of members</returns>
        [HttpGet]
        public object GetAllMembers1()
        {
            var memberGroupService = Services.MemberGroupService;
            var memberService = Services.MemberService;
            List<IMember> members = new List<IMember>();
            List<UMember> _members = new List<UMember>();
           var roles = memberGroupService.GetAll();
           foreach (var role in roles)
           {
               var groupMembers = memberService.GetMembersByGroup(role.Name);
               if (groupMembers != null && groupMembers.Count() > 0){
                 foreach(var member in groupMembers){
              _members.Add(new UMember(){
                 Id = member.Id,
                  Key = member.Key,
                  Name  = member.Name,
                 Login = member.Username,
                 LastLogin = member.LastLoginDate,
                 CreatedOn = member.CreateDate,
                 UpdatedOn = member.UpdateDate,
                 Type = member.ContentTypeAlias,
                 Email = member.Email,
                 Group = role.Name
                });
           }
               }
           }
            int membersCount = 0;
            members = memberService.GetAll(0, 1000000000,
            out membersCount).ToList();
            foreach (var member in members)
            {
                int index = _members.FindIndex(item => item.Id == member.Id);
                if (index < 0)
                {
                    _members.Add(new UMember()
                    {
                        Id = member.Id,
                        Key = member.Key,
                        Name = member.Name,
                        Login = member.Username,
                        LastLogin = member.LastLoginDate,
                        CreatedOn = member.CreateDate,
                        UpdatedOn = member.UpdateDate,
                        Type = member.ContentTypeAlias,
                        Email = member.Email,
                        Group = ""
                    });
                }

            }
                return _members;
        }

        /// <summary>
        /// Gets a list of all Umbraco member roles
        /// </summary>
        /// <returns>Returns all roles</returns>
        [HttpGet]
        public IEnumerable<string> GetAllRoles()
        {
            var memberService = Services.MemberService;
            return memberService.GetAllRoles();
        }

    }

}