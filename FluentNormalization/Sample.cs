using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentNormalization
{
    internal class Sample
    {
        public string SampleProperty { get; set; }
    }

    internal class SampleValidator : AbstractValidator<Sample>
    {
        public SampleValidator()
        {
            RuleFor(x => x.SampleProperty).NotNull().NotEmpty();
        }
    }
}
