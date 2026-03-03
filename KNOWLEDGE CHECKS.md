# Umbraco MVC

Course contains several knowledge checks exercises. Below are the questions presented in these knowledge checks.

## Exercise 1: Models Builder

1. *How can you access IPublishedContent in the Template view?*
   - Using `@Model.`

2. *Models Builder is used to compile Document Types into C# classes, making them available in Controllers, Views, and other parts of the application.*
   - True.

3. *Which sentence is correct?*
   - ModelsMode specifies the mode of Models Builder, while ModelsDirectory defines the folder location where the models will be generated.

4. *Drag the words from the right side into the correct boxes in the sentence.*
   - With the generic IPublishedContent, there is no explicit page model, so you need to use the `Value` extension method like `@Model.Value("property")`.
   - With strongly typed IPublishedContent, you can reference a Document Type model and directly access properties like `@Model.Property`.