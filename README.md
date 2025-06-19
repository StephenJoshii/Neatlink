# NeatLink - A Simple URL Shortener in .NET

NeatLink is a lightweight, minimal API built with .NET 8 that provides a fast and simple URL shortening service. This project was built from scratch to demonstrate a practical backend service, including API design, data handling, and local development setup.

## Features

-   **Shorten URL**: Accepts a long URL and returns a unique, 6-character short code.
-   **Redirect**: Redirects users from the short URL to the original long URL.
-   **API Documentation**: Integrated Swagger UI for easy, interactive API testing.

## Tech Stack

-   **Framework**: .NET 8 (Minimal APIs)
-   **Language**: C#
-   **Database**: Entity Framework Core with an In-Memory Provider
-   **API Testing**: Swagger / Swashbuckle

## How to Run Locally

1.  **Prerequisites**:
    -   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or newer.

2.  **Clone the repository**:
    ```bash
    git clone [https://github.com/StephenJoshii/NeatLink.git](https://github.com/StephenJoshii/NeatLink.git)
    cd NeatLink
    ```

3.  **Run the application**:
    ```bash
    dotnet run --launch-profile http
    ```

4.  **Access the API**:
    -   The API will be running on the port specified in the terminal (e.g., `http://localhost:5027`).
    -   Navigate to `http://localhost:PORT/swagger` to access the interactive Swagger UI and test the endpoints.

## API Endpoints

### 1. Shorten a URL

-   **Method**: `POST`
-   **Endpoint**: `/shorten`
-   **Request Body**:
    ```json
    {
      "url": "[https://www.your-long-url-goes-here.com/](https://www.your-long-url-goes-here.com/)"
    }
    ```
-   **Success Response** (`200 OK`):
    ```json
    {
      "shortUrl": "http://localhost:5027/aB1xYz"
    }
    ```

### 2. Redirect to Original URL

-   **Method**: `GET`
-   **Endpoint**: `/{shortCode}` (e.g., `/aB1xYz`)
-   **Action**: Redirects the user's browser to the original URL with a 308 Permanent Redirect status.