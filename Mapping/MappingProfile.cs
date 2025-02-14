using AutoMapper;
using BackendDB.Models;
using BackendDB.ModelDTOs;
using APIMain.Authentication.UserDataObjects;

namespace APIMain.Mapping {
    /// <summary>
    /// Creates mappings between DB types and their DTO types
    /// </summary>
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<BlogPost, BlogPostDTO>().ReverseMap();
            CreateMap<Chat, ChatDTO>().ReverseMap();
            CreateMap<ChatMessage, ChatMessageDTO>().ReverseMap();
            CreateMap<ChatMessageReaction, ChatMessageReactionDTO>().ReverseMap();
            CreateMap<Language, LanguageDTO>().ReverseMap();
            CreateMap<Locale, LocaleDTO>().ReverseMap();
            CreateMap<Project, ProjectDTO>().ReverseMap();
            CreateMap<ProjectFile, ProjectFileDTO>().ReverseMap();
            CreateMap<ProjectFolder, ProjectFolderDTO>().ReverseMap();
            CreateMap<SourceString, SourceStringDTO>().ReverseMap();
            CreateMap<SourceStringContext, SourceStringContext>().ReverseMap();
            CreateMap<SourceStringTag, SourceStringTagDTO>().ReverseMap();
            CreateMap<BackendDB.Models.Task, TaskDTO>().ReverseMap();
            CreateMap<Translation, TranslationDTO>().ReverseMap();
            CreateMap<TranslationVote, TranslationVoteDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserLogin, UserLoginDTO>().ReverseMap();

            CreateMap<User, UserSignUpData>().ReverseMap();
            CreateMap<User, UserLoginData>().ReverseMap();
        }
    }
}
