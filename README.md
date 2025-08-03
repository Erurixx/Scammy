# Scammy - A Cloud-Based Platform for Online Job Scam Reporting and Awareness
Scammy is a cloud-based web application designed to combat the growing problem of online job scams. The platform allows users to report suspicious job advertisements, access scam awareness articles, and explore scam trends. It is developed as part of a university cloud computing assignment to demonstrate the integration of cloud services, database solutions, and role-based access. This system features three distinct user roles—Jobseeker, Admin, and Analyst—each responsible for different functionalities. It is hosted on AWS infrastructure and utilizes cloud-native services such as EC2, RDS, and optionally S3 and CloudWatch.

# Key Features
Jobseeker:
1. Submit reports on suspicious or fraudulent job ads.
2. View and search a database of public scam reports.
3. Receive awareness tips and scam prevention guidance.

Admin:
1. Manage incoming scam reports (approve/reject/delete).
2. Monitor user submissions and perform content moderation.
3.Access dashboard statistics and activity logs.

Analyst (Content Creator):
1. Publish scam awareness articles on current scam trends.
2. Tag articles by category (e.g., WhatsApp, Telegram, Recruitment scams).
3. Export scam data (e.g., monthly statistics).
4. View audit logs of published content.

# Technologies Used
Frontend:
1. HTML5, CSS3, JavaScript
2. ASP.NET MVC (Razor Views)

Backend:
1. ASP.NET MVC (C#)
2. Entity Framework (optional for DB interaction)

# Acknowledgments
This system is developed as part of the Cloud Computing group assignment at Asia Pacific University, Year 3. The goal is to help the public stay informed about job scams while showcasing practical use of cloud development and deployment strategies.
