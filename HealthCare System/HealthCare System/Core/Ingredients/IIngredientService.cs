﻿using HealthCare_System.Core.Ingredients.Model;
using HealthCare_System.Core.Ingredients.Repository;
using System.Collections.Generic;

namespace HealthCare_System.Core.Ingredients
{
    public interface IIngredientService
    {
        IngredientRepo IngredientRepo { get; }

        void Create(IngredientDto ingredientDto);
        void Delete(Ingredient ingredient);
        List<Ingredient> Ingredients();
        bool IsIngredientAvailableForChange(Ingredient ingredient);
        void Update(IngredientDto ingredientDto, Ingredient ingredient);
    }
}