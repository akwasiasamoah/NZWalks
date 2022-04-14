using FluentValidation;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDifficultyRequest : AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyRequest()
        {
            RuleFor(x => x.code).NotEmpty();
        }
    }
}
