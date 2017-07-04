using System;
using System.Threading.Tasks;


using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
    // Variables Declaration
    
    public int currentLevel = 2;
    
    public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
    }


    // None Intent is reached when none of the other is called
    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
            
       var reply = context.MakeMessage();
        reply.Text = $" Did you said : '{result.Query}' ?";
        //reply.Speak = $"Default Intent Reached. You may have said something not handled";
        reply.Speak = $"Default Intent reached";
        
        await context.PostAsync(reply); //
        context.Wait(MessageReceived);
    }
    

    // ResValue is used to determine the value of a component
    // If the bot understand the name of the component, a specific procedure is launched
    // For example, if 'resistor' is found, the three colours will be asked 
    [LuisIntent("ResValue")]
    public async Task ResValue(IDialogContext context, LuisResult result)
    {
        // Detection des entités qui pourraient être celles de 
        EntityRecommendation getComponent;
        
        //Création de la variable de réponse
        var reply = context.MakeMessage();
        
        if(result.TryFindEntity("component",out getComponent))
        {
            /*On créé une variable contenant l'entité de l'intent reconnue
            componentFound contient le mot de la requete identifié comme une entité 'component'
            */
            var componentFound = getComponent.Entity;
            
             switch (componentFound)
      {
         case "resistor":
            reply.Text = $"I start the "+componentFound+" identification ";
            reply.Speak = $"ResValue intent. You said: {result.Query}";
            //Launch identification
            break;
            
         case "capacitor":
            reply.Text = $"I start the "+componentFound+" identification ";
            reply.Speak = $"ResValue intent. You said: {result.Query}";
            //Launch identification
            break;
            
         default:
            reply.Text = $"The value of the component "+ componentFound  +" is unknown.";
            break;   
      }

        }
        
        else {
            reply.Text = $"Sorry, but wich component do you want to know the value ?";
            reply.Speak = $"ResValue intent. You said: {result.Query}";
        }
        
        await context.PostAsync(reply); //
        context.Wait(MessageReceived);
    }


    // ComponentPosition
    [LuisIntent("ComponentPosition")]
    public async Task ComponentPosition(IDialogContext context, LuisResult result)
    {
        EntityRecommendation getComponent;
        
        //Création de la variable de réponse
        var reply = context.MakeMessage();
        
        if(result.TryFindEntity("component",out getComponent))
        {
            /*On créé une variable contenant l'entité de l'intent reconnue
            componentFound contient le mot de la requete identifié comme une entité 'component'
            */
            var componentFound = getComponent.Entity;
            
             switch (componentFound)
      {
         case "resistor":
            reply.Text = $"This "+componentFound+" goes here ";
            reply.Speak = $"ResValue intent. You said: {result.Query}";
            //indicate position
            break;
            
         case "capacitor":
            reply.Text = $"This "+componentFound+" goes here ";
            reply.Speak = $"ResValue intent. You said: {result.Query}";
            //indicate position
            break;
            
         default:
            reply.Text = $"The position of the component "+ componentFound  +" is unknown.";
            break;   
      }

        }
        
        else {
            reply.Text = $"Sorry, but wich component do you want to know the position ?";
            reply.Speak = $"Componentposition intent. You said: {result.Query}";
        }
        
        await context.PostAsync(reply); //
        context.Wait(MessageReceived);
    }
    
    
    // Help undersrtand all the general (not specifics) questions asked by the player
    #pragma warning disable 1998
    [LuisIntent("OnDevice.Help")]
    public async Task Help(IDialogContext context, LuisResult result)
    {
        var promptOptions = new PromptOptions<string>(
                "Do you want to know what I can I do for you ? (yes/no)",
                "Do you want to know what I can I do for you ? You can say either Yes or No");
            PromptDialog.Confirm(context, this.HelpAsync, promptOptions);

    }
    private async Task HelpAsync(IDialogContext context, IAwaitable<bool> result)
        {
            
            var r = await result;
            
            if (r)
            {
                await context.PostAsync("Here are some questions you can ask to me : \t Value of a component | \t Position of a component | \t Launch a new level | \t Identification of a resistor by the colours | \t Quit the game | \t Ask for a formula",
                    "Here are some questions you can ask to me");
            }
            else
            {
                await context.PostAsync("Ok, you can continue the game",
                    "Ok,you can continue the game");
                    // Nothing happens
            }

            context.Wait(MessageReceived);
        }
    #pragma warning restore 1998 
    
    // LaunchLevel is used to change or launch a level, the bot will identify the level and ask for a confirmation
    [LuisIntent("LaunchLevel")]
    public async Task LaunchLevel(IDialogContext context, LuisResult result)
    {
        EntityRecommendation getLevelName;
        
        //Création de la variable de réponse
        var reply = context.MakeMessage();
        
        if(result.TryFindEntity("LevelName",out getLevelName))
        {
            /*On créé une variable contenant l'entité de l'intent reconnue
            componentFound contient le mot de la requete identifié comme une entité 'component'
            */
            var LevelNameFound = getLevelName.Entity;
            
            switch (LevelNameFound)
                {
                 case "one": case "1":
                    //reply.Text = $"Ok, I launch level one";
                    //reply.Speak = $"You said: {result.Query}";
                    //Code pour lancer niveau 1
                    SelectLevel(1,reply);
                    break;
                    
                 case "two": case "2":
                    //reply.Text = $"Ok, I launch level two";
                    //reply.Speak = $"You said: {result.Query}";
                    //Code pour lancer niveau 2
                    SelectLevel(2,reply);
                    break;
                    
                 case "three": case "3":
                    //reply.Text = $"Ok, I launch level three";
                    //reply.Speak = $"You said: {result.Query}";
                    //Code pour lancer niveau 3
                    SelectLevel(3,reply);
                    break;
            
                 default:
                    reply.Text = $"The level "+ LevelNameFound  +" is unknown.";
                    //Faire répéter pour savoir quel niveau lancer
                    break;   
                }
            
        }
        
        else 
            {
                reply.Text = $"Sorry, I don't understand the level you want to launch";
                reply.Speak = $"You said: {result.Query}";
                //Faire répéter pour savoir quel niveau lancer
            }
        
        await context.PostAsync(reply); //
        context.Wait(MessageReceived);
    }
    
    #pragma warning disable 1998
    private async Task SelectLevel(int levelExpected, Microsoft.Bot.Connector.IMessageActivity reply){
        
            if(levelExpected == currentLevel){
                reply.Text = $"Ok, I restart the current level ";
                reply.Speak = $"Ok, I restart the current level";
            }
            else{
                reply.Text = $"Ok, I launch level "+ levelExpected;
                reply.Speak = $"Ok, I launch level "+ levelExpected;
                currentLevel = levelExpected;
            }
    }
    #pragma warning restore 1998
    
    
    
    // ResColour is launched when three colours are said by the player
    // The intent associates the colours to a value of resistor
    [LuisIntent("ResColour")]
    public async Task ResColour(IDialogContext context, LuisResult result)
    {
        EntityRecommendation getColourOne;
        EntityRecommendation getColourTwo;
        EntityRecommendation getColourThree;
        
        //Création de la variable de réponse
        var reply = context.MakeMessage(); 
        
        // These two next variables :
        // Final contains the value of the resistance 
        // TextToSpeak contains the information wich has to be talk to the user
        float Final = 0;
        float testNull = 0;
        String TextToSpeak = "ResColour Intent Error, please reformulate your intent";
        String TextErrorColour1 = "";
        String TextErrorColour2 = "";
        String TextErrorColour3 = "";
        
        int associatedValue1 = 0;
        int associatedValue2 = 0;
        int associatedValue3 = 0;
        
        
   //------------------------------------------------------------------------------------------//
   //               USED TO DETERMINE THE VALUE OF A RESISTOR THANKS TO ITS COLOURS            //
   //------------------------------------------------------------------------------------------//
        
        // Three Colour have to be understood by the bot in order to stert the recognition
        if(result.TryFindEntity("Colour1",out getColourOne) && result.TryFindEntity("Colour2",out getColourTwo) && result.TryFindEntity("Colour3",out getColourThree))
        {
            // These entities represent the tree colours listened by the bot
            var FirstColour = getColourOne.Entity;
            var SecondColour = getColourTwo.Entity;
            var ThirdColour = getColourThree.Entity;
            
            // Allows the association between text and int accordingly to the colour        
            switch (FirstColour)
                {
                    case "black":
                        associatedValue1 = 0;
                        break;
        
                    case "brown":
                        associatedValue1 = 1;
                        break;
                        
                    case "red":
                        associatedValue1 = 2;
                        break;
                        
                    case "orange":
                        associatedValue1 = 3;
                        break;
                        
                    case "yellow":
                        associatedValue1 = 4;
                        break;
                        
                    case "green":
                        associatedValue1 = 5;
                        break;
                        
                    case "blue":
                        associatedValue1 = 6;
                        break;
                       
                    case "violet": case "purple":
                        associatedValue1 = 7;
                        break;
                        
                    case "grey":
                        associatedValue1 = 8;
                        break;
                        
                    case "white":
                        associatedValue1 = 9;
                        break;
                        
                    default:
                        TextErrorColour1 = " but this value may be wrong, I was unable to recognize the first colour ";
                        break;
                }
                
            // Allows the association between text and int accordingly to the colour        
            switch (SecondColour)
                {
                    // Allows the association between text and int accordingly to the colour
                
                    case "black":
                        associatedValue2 = 0;
                        break;
        
                    case "brown":
                        associatedValue2 = 1;
                        break;
                        
                    case "red":
                        associatedValue2 = 2;
                        break;
                        
                    case "orange":
                        associatedValue2 = 3;
                        break;
                        
                    case "yellow":
                        associatedValue2 = 4;
                        break;
                        
                    case "green":
                        associatedValue2 = 5;
                        break;
                        
                    case "blue":
                        associatedValue2 = 6;
                        break;
                       
                    case "violet": case "purple":
                        associatedValue2 = 7;
                        break;
                        
                    case "grey":
                        associatedValue2 = 8;
                        break;
                        
                    case "white":
                        associatedValue2 = 9;
                        break;
                        
                    default:
                        TextErrorColour2 = " but this value may be wrong, I was unable to recognize the second colour ";
                        break;
                } 
                
            // The third colour indicate the multiplier to use    
            switch (ThirdColour)
                {
                    case "black":
                        associatedValue3 = 1;
                        break;
                    
                    case "brown":
                        associatedValue3 = 10;
                        break;
                    
                    case "red":
                        associatedValue3 = 100;
                        break;
                   
                    case "orange":
                        associatedValue3 = 1000;
                        break;
                    
                    case "yellow":
                        associatedValue3 = 10000;
                        break;
                   
                    case "green":
                        associatedValue3 = 100000;
                        break;
                   
                    case "blue":
                        associatedValue3 = 1000000;
                        break;
                    
                    case "violet": case "purple":
                        associatedValue3 = 10000000;
                        break;
                        
                    default:
                        TextErrorColour3 = "but this value may be wrong, I was unable to recognize the third colour ";
                        break;
                }    
                
             // Calculate the value of the resistor and create the text to speak
            Final = (associatedValue1*10 + associatedValue2) * associatedValue3;
            testNull = Final;
            TextToSpeak = "This is a " + Final + " Ohms resistor";
            
            // To avoid the case of a value with too much '0', a conversion is done 
            
            // If the value is between 1k and 1M, the suffix 'kilo' is associated
            if (Final > 1000 && Final < 1000000)
                {
                    Final = Final / 1000;
                    TextToSpeak = "This is a " + Final + " kilo Ohms resistor";
                } 
            
            // If the value is superior of 1M, the suffix 'Mega' is associated
            if (Final >= 1000000)
                {
                    Final = Final / 1000000;
                    TextToSpeak = "This is a " + Final + " Mega Ohms resistor";
                } 
                
        }
        
        if(testNull == 0)
        {
            
        reply.Text = $"The result of the analysis return a null value, it may has been caused by a misunderstood colour";
        reply.Speak = $"The result of the analysis return a null value, please try again";
        
        }
        else
        {
            
        // Creating answer
        reply.Text = $""+ TextToSpeak + TextErrorColour1 + TextErrorColour2 + TextErrorColour3;
        reply.Speak = $""+ TextToSpeak + TextErrorColour1 + TextErrorColour2 + TextErrorColour3;
        
        await context.PostAsync(reply); //
        context.Wait(MessageReceived);
        
       }
    }
    
    
    // Formula allows the player to have indications on the formulas of the level
    [LuisIntent("Formula")]
    public async Task Formula(IDialogContext context, LuisResult result)
    {
        
        EntityRecommendation getFormulaName;
        
        //Création de la variable de réponse
        var reply = context.MakeMessage();
        
        if(result.TryFindEntity("FormulaEntity",out getFormulaName))
        {
            var FormulaFound = getFormulaName.Entity;
            
            switch (FormulaFound)
                {
                 case "voltage":
                    reply.Text = $"Ok, here is the output voltage formula";
                    reply.Speak = $"Ok, here is the output voltage formula";
                    // launch output voltage formula
                    break;
                    
                 case "equivalent":
                    reply.Text = $"Ok, here is the equivalent resistor formula";
                    reply.Speak = $"Ok, here is the equivalent resistor formula";
                    // Launch Equivalent Resistor Formula
                    break;
                    
                case "frequency":
                    reply.Text = $"Ok, here is the frequency formula";
                    reply.Speak = $"Ok, here is the frequency formula";
                    // Launch frequency Formula
                    break;
            
                 default:
                    reply.Text = $"The formula "+ FormulaFound  +" is unknown.";
                    //Faire répéter pour savoir quel niveau lancer
                    break;   
                }
            
        }
        
        else 
            {
                reply.Text = $"Sorry, I don't understand the formula you want to know";
                reply.Speak = $"I don't understand the formula you want to know";
                //Faire répéter pour savoir quelle formule lancer
            }
        
        await context.PostAsync(reply); //
        context.Wait(MessageReceived);
         
    }
    
    
    /*
    ISSUE ENCOUNTERED
    warning CS1998: This async method lacks 'await' operators and will run synchronously.
    Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)'
    to do CPU-bound work on a background thread.
    
    ---> suppress the warning with #pragma 1998
    */
    
    // ReturnQuit allows the player to quit the game and go back to the main menu
    // A confirmation is required, the player can either resume the game or launch the main menu
    #pragma warning disable 1998
    [LuisIntent("ReturnQuit")]
        public async Task ReturnQuit(IDialogContext context, LuisResult result)
        {
            var promptOptions = new PromptOptions<string>(
                "Do you really want to quit ? (yes/no)",
                "Do you really want to quit ? You can say either Yes or No");
            PromptDialog.Confirm(context, this.AfterConfirmAbsenceAsync, promptOptions);
        }

        private async Task AfterConfirmAbsenceAsync(IDialogContext context, IAwaitable<bool> result)
        {
            var r = await result;
            if (r)
            {
                await context.PostAsync("Ok, I launch the main menu.",
                    "Ok, I launch the main menu.");
                    // launch the scene corresponding to the main menu
            }
            else
            {
                await context.PostAsync("Ok, you can continue the game",
                    "Ok,you can continue the game");
                    // Nothing happens
            }

            context.Wait(MessageReceived);
        }
    #pragma warning restore 1998  
    
    
        /* Bloc de test de type log
    var reply1 = context.MakeMessage();
    reply1.Text = $"oui";
    await context.PostAsync(reply1);//
    */
}