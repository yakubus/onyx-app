namespace Identity.Application.Contracts.Messaging.Emails;

internal static class EmailTemplates
{
    internal static (string subject, string htmlBody, string plainTextBody) EmailVerificationBodyTemplate(string code, string username) => (
        "Verify your account",
        $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8">
            <title>Email Verification</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }
                .header {
                    background-color: #4CAF50;
                    color: #ffffff;
                    padding: 10px 20px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                }
                .content {
                    padding: 20px;
                    font-size: 16px;
                    line-height: 1.6;
                }
                .verification-code {
                    display: block;
                    width: fit-content;
                    margin: 20px auto;
                    padding: 10px 20px;
                    background-color: #f4f4f4;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                    font-size: 24px;
                    font-weight: bold;
                }
                .footer {
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #aaaaaa;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <div class="header">
                    <h1>Email Verification</h1>
                </div>
                <div class="content">
                    <p>Hello [User],</p>
                    <p>Thank you for registering with us. Please use the verification code below to verify your email address.</p>
                    <div class="verification-code">
                        {{code}}
                    </div>
                    <p>If you did not request this, please ignore this email.</p>
                    <p>Best regards,<br>Onyx</p>
                </div>
                <div class="footer">
                    <p>© 2024 Onyx All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        """,
        $"""
        Email Verification
        Hello {username}
        Thank you for registering with us. 
        Please use the verification code below to verify your email address.
        {code}
        If you did not request this, please ignore this email.
        Best regards, Onyx 
        © 2024 Onyx All rights reserved.
        """);
    internal static (string subject, string htmlBody, string plainTextBody) EmailChangeBodyTemplate(string code, string username) => (
        "",
        $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8">
            <title>New Email Verification</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }
                .header {
                    background-color: #4CAF50;
                    color: #ffffff;
                    padding: 10px 20px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                }
                .content {
                    padding: 20px;
                    font-size: 16px;
                    line-height: 1.6;
                }
                .verification-code {
                    display: block;
                    width: fit-content;
                    margin: 20px auto;
                    padding: 10px 20px;
                    background-color: #f4f4f4;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                    font-size: 24px;
                    font-weight: bold;
                }
                .footer {
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #aaaaaa;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <div class="header">
                    <h1>New Email Verification</h1>
                </div>
                <div class="content">
                    <p>Hello {{username}},</p>
                    <p>You made a request for changing an email address to this one. No problem! Here is the verification code that will make your dreams come true.</p>
                    <div class="verification-code">
                        {{code}}
                    </div>
                    <p>If you did not request this, please ignore this email.</p>
                    <p>Best regards,<br>Onyx</p>
                </div>
                <div class="footer">
                    <p>© 2024 Onyx All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        """,
        $"""
        New Email Verification
        Hello {username} 
        You made a request for changing an email address to this one.
        No problem! Here is the verification code that will make your dreams come true.
        {code}
        If you did not request this, please ignore this email.
        Best regards, Onyx 
        © 2024 Onyx All rights reserved.
        """);
    internal static (string subject, string htmlBody, string plainTextBody) ForgotPasswordBodyTemplate(string code) => (
        "",
        $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="UTF-8">
            <title>New Password Verification</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }
                .header {
                    background-color: #4CAF50;
                    color: #ffffff;
                    padding: 10px 20px;
                    text-align: center;
                    border-radius: 8px 8px 0 0;
                }
                .content {
                    padding: 20px;
                    font-size: 16px;
                    line-height: 1.6;
                }
                .verification-code {
                    display: block;
                    width: fit-content;
                    margin: 20px auto;
                    padding: 10px 20px;
                    background-color: #f4f4f4;
                    border: 1px solid #dddddd;
                    border-radius: 4px;
                    font-size: 24px;
                    font-weight: bold;
                }
                .footer {
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #aaaaaa;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <div class="header">
                    <h1>New Password Verification</h1>
                </div>
                <div class="content">
                    <p>Hello [User],</p>
                    <p>We are sorry to hear that you forgot your password, but don't worry! Here is your verification code to set your brand new password</p>
                    <div class="verification-code">
                        {{code}}
                    </div>
                    <p>If you did not request this, please ignore this email.</p>
                    <p>Best regards,<br>Onyx</p>
                </div>
                <div class="footer">
                    <p>© 2024 Onyx All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        """,
        $"""
        New Password Verification
        Hello
        We are sorry to hear that you forgot your password, but don't worry!
        Here is your verification code to set your brand new password.
        {code}
        If you did not request this, please ignore this email.
        Best regards, Onyx 
        © 2024 Onyx All rights reserved.
        """);
}