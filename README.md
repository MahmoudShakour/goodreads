<div align="center">
    <h1 align='center'><i>GoodReads clone</i></h1>
    <p>Goodreads is a popular online platform designed for book lovers. It allows users to review, rate, and discuss books. </p>
</div>

</details>
<hr>
<h2 href="#BuiltWith">Built With : </h2>
 <ul>
  <li>ASP.NET 8.0</li>
  <li>SQL Server</li>
  <li>EF Core</li>
  <li>Linq</li>
 </ul>
<hr>
<h2 href="#GettingStarted">Getting Started</h2>
<blockquote>
  <p>This is a list of needed steps to set up your project locally, to get a local copy up and running follow these instructions.
 </p>
</blockquote>
<ol>
  <li><strong><em>Clone the repository</em></strong>
    <div>
        <code>$ git clone git@github.com:MahmoudShakour/goodreads.git </code>
    </div>
  <li><strong><em>Install dependencies</em></strong>
    <div>
        <code>$ dotnet restore</code>
    </div>
  </li>
  <li><strong><em>migrate database</em></strong>
    <div>
        <code>$ dotnet ef database update</code>
    </div>
  </li>
  <li><strong><em>Start the application</em></strong>
    <div>
        <code>$ dotnet watch run</code>
    </div>
  </li>

</ol>
<hr>

<h2 href="#API-Documentation">API Documentation</h2>
<blockquote>
  <p>
  You can look on the API documentation at <a href="https://app.swaggerhub.com/apis-docs/MAHMOUDSHAKOURDEV/goodreads-api/v1">API Documentation</a>
  </p>
</blockquote>
<hr>

## 📷 Features

<details>
<summary>
<h4 style="display:inline">
<strong><em> User Authentication</em></strong></h4>
</summary>

- You can use sign up and sign in.

</details>

<details>
<summary>
<h4 style="display:inline">
<strong><em> Author</em></strong></h4>
</summary>

- only admin can create/edit/delete authors
- user can get authors

</details>

<details>
<summary>
<h4 style="display:inline">
<strong><em> Book</em></strong></h4>
</summary>

- only admin can create/edit/delete books
- user can get books

</details>

<details>
<summary>
<h4 style="display:inline">
<strong><em> Review</em></strong></h4>
</summary>

- users can write review to books.
- only the user who wrote the review can edit or delete it.
- other users can get reviews of particular book or particular user
</details>

<details>
<summary>
<h4 style="display:inline">
<strong><em> Comment</em></strong></h4>
</summary>

- users can write comments on the reviews.
- only the user who wrote the comment can edit or delete it.
- other users can get comments of particular review or particular user

</details>

<details>
<summary>
<h4 style="display:inline">
<strong><em> Like</em></strong></h4>
</summary>

- users can like reviews.
- only the user who make the like can delete it.
- other users can get likes of particular review or particular user

</details>

<details>
<summary>
<h4 style="display:inline">
<strong><em> Rating</em></strong></h4>
</summary>

- users can rate books.
- only the user who rate the book can edit or delete it.
- other users can get rating of particular book or particular user

</details>
