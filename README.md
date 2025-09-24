## ravendb-sales-assistant-demo
This repository demonstrates how to use [RavenDB](https://ravendb.net) and AI Features as a sales assistant utility.

### Before you start

On Test Unit layer, you'll find two `.csv` files that you can use to import sample data into your RavenDB instance. If you want to use your own data, keep the headers in the same format.

Run the application and create the Message templates using `List1` or `List2` parameters create the message sample you want to generate to your leads.
Then you can import the data from the CSV files into your RavenDB instance from `LeadsController`, selecting the `List1` or `List2` option and the Language that your leads list speak.

Then, when you get the paginated Messages on message resource, you'll be able to see what GenAI has generated as massage to your leads. It's on HTML format, so get the endpoint and load it directly on your browser.

### What's about to come

I'll insert the GetAI configurations from the Clients soon. Then, the application will be able to be used without a preconfigured database dump, that I cannot provide yet.

### Enjoy it!

Let me know if you have any questions or suggestions.
- [Lucas Tavares](https://www.linkedin.com/in/lucasrtavares/)
- [lucas.tavares@ravendb.net](mailto:lucas.tavares@ravendb.net)