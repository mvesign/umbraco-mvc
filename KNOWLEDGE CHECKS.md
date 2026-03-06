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

## Exercise 4: Surface Controllers

1. *When using Surface Controllers for forms, which helper method must be used?*
   - `@html.BeginUmbracoForm`

2. *What information does the Surface Controller get when using `@html.BeginUmbracoForm`?*
   - User culture and anti-forgery tokens.

3. *What are Surface Controllers primarily used for in Umbraco?*
   - Working with HTML/MVC forms on an Umbraco page.

## Exercise 5: Web API Controllers

1. *Web API controllers in Umbraco are primarily used for building:*
   - REST services.

2. *The Web API in Umbraco is built on top of the ASP.NET Web API.*
   - True.

3. *What is the base class that Web API controllers inherit from in Umbraco 14 and above?*
   - `ControllerBase`

4. *Drag the words from the right side into the correct boxes in the sentence.*
   - In Umbraco 14 and above, you must `manually authorize` requests against either a backoffice `user` or a logged-in `member`, as the default controllers no longer `automatically` check for `authorization`.

## Exercise 6: Content API

1. *How do you save and publish content in Umbraco programmatically?*
   - Using `_contentService.Save(content);` and `_contentService.Publish(content, ["*"]);`.

2. *What are the mandatory properties when creating content programmatically?*
   - `Name`
   - `ParentId`
   - `DocumentTypeAlias`

3. *When creating or updating nodes in Umbraco, you can set custom properties using the `SetValue` method, which requires the property alias and the input value.*
   - True;

4. *What does the asterisk (*) in `_contentService.Publish(content, ["*"]);` indicate?*
   - It means to publish all cultures.

5. *Drag the words from the right side into the correct boxes in the sentence.*
   - When using ContentService in `Views`, it makes calls to the `database`, so it’s recommended to use `UmbracoHelper` to access the published `cache`.

6. *What can you do with services in Umbraco?*
   - Create, delete, and update: content, Document Types, and Data Types programmatically.

7. *The `ServiceContext` in Umbraco is used to access various services such as content, media, and member management.*
   - True.

## Exercise 7: Media API

1. *What are the mandatory properties when creating media programmatically?*
   - `Name`, `ParentId`, `MediaTypeAlias`.

2. Why do we need to use `TemporaryFileService` when working with media programmatically in Umbraco?
  - To automatically clean up temporary files and maintain a tidy folder, improving performance.

3. What method is used when creating media items programmatically?
   - `CreateMedia()`

## Exercise 8: Composing Umbraco with Services and Notifications

1. **