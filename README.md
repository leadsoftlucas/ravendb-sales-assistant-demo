## ravendb-sales-assistant-demo
This repository demonstrates how to use [RavenDB](https://ravendb.net) and AI Features as a sales assistant utility.

### Before you start

On Test Unit layer, you'll find two `.csv` files that you can use to import sample data into your RavenDB instance. If you want to use your own data, keep the headers in the same format.

Run the application and create the Message templates using `List1` or `List2` parameters create the message sample you want to generate to your leads.
Then you can import the data from the CSV files into your RavenDB instance from `LeadsController`, selecting the `List1` or `List2` option and the Language that your leads list speak.

Then, when you get the paginated Messages on message resource, you'll be able to see what GenAI has generated as massage to your leads. It's on HTML format, so get the endpoint and load it directly on your browser.

### Whats inside

Basically, this is a very simple application that demonstrates how to use RavenDB and GenAI to generate messages to leads. If you have a couple of e-mail templates in your preffered language, you can use it to create messages based on a leads database that you can collect from spreasheets, other applications, integrations, whatever...
So from two collections: `Leads` and `Templates`, GenAI will first and enrich your Leads with more information, and then Index will create an artificial collection with the Messages suggestion based on the template you provided for each kind of Lead you have, based on the template for that List. The e-mail would be translated to the language of you lead and adjusted with client information, best e-mail on Professional or Personal scenarios and adjust the e-mail voice tone based on gender and person generation (extracted from age).

So you'll have personalized messages, subjects and the recipients for your leads, ready to be sent. A next step could be integrate with SES or Sengrid or similars to shot e-mail or other messaging platforms you wish.

### What's about to come

I'll insert the GetAI configurations from the Clients soon. Then, the application will be able to be used without a preconfigured database dump, that I cannot provide yet.

I'll include here soon, the used AI prompts and the proper Index to create the Messages collection.

### Enjoy it!

Let me know if you have any questions or suggestions.
- [Lucas Tavares](https://www.linkedin.com/in/lucasrtavares/)
- [lucas.tavares@ravendb.net](mailto:lucas.tavares@ravendb.net)