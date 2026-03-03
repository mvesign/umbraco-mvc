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

## Exercise 2: Render Controllers and Route Hijacking

1. *When is PublishedContentWrapped typically used in Umbraco?*
   - When building a custom model upon the existing PublishedContent model.

2. *Route hijacking in Umbraco allows you to override the default controller for a specific document type by implementing a custom controller, giving you control over how content is rendered.*
   - True.

3. *ViewModels help separate business logic from view logic.*
   - True.

4. *Which controller should a custom controller inherit from to handle route hijacking in Umbraco?*
   - `RenderController`

## Exercise 3: Member Authentication

1. *What else is needed for a member-based authentication implementation in Umbraco?*
   - Setting up a Partial View and enabling public access to pages you want to restrict.

2. *Which helper method is used to set up the basic login form in Umbraco?*
   - `BeginUmbracoForm`

3. *Which of the following best describes the purpose of member-based authentication?*
   - It restricts certain pages to logged-in members only.

4. *The login form in Umbraco is associated with which controller?*
   - `UmbLoginController`