using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Persistence;

namespace Application.Photos
{
    public class AddPhoto
    {
        public class Comand : IRequest<Result<Photo>>
        {
            public required IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Comand, Result<Photo>>
        {
            private readonly DataContext m_Context;
            private readonly IPhotoAccessor m_PhotoAccessor;
            private readonly IUserAccessor m_UserAccessor;

            public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                m_Context = context;
                m_PhotoAccessor = photoAccessor;
                m_UserAccessor = userAccessor;
            }

            public async Task<Result<Photo>> Handle(Comand request, CancellationToken cancellationToken)
            {
                var user = await m_Context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == m_UserAccessor.GetUsername());
                if (user == null) return null;

                var photoUploadResult = await m_PhotoAccessor.AddPhoto(request.File);
                var photo = new Photo
                {
                    URL = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if (!user.Photos.Any(x => x.IsMain))
                {
                    photo.IsMain = true;
                }
                user.Photos.Add(photo);

                var result = await m_Context.SaveChangesAsync() > 0;

                if (result)
                {
                    return Result<Photo>.Succes(photo);
                }

                return Result<Photo>.Failure("Problem adding photo");
            }
        }
    }
}