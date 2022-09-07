// See https://aka.ms/new-console-template for more information
using System.Reflection;

// this works
// B b = new B();
// var members = b.GetType().GetMembers(BindingFlags.GetField | BindingFlags.GetProperty).Select(x => x.Name);

// this doesn't work
// https://stackoverflow.com/questions/45990426/get-all-properties-of-an-implemented-interface
var members = typeof(IB).GetMembers(BindingFlags.GetField | BindingFlags.GetProperty).Select(x => x.Name);
foreach (var m in members)
{
    Console.WriteLine(m);
}

class A
{
    public void Do() { }
}

class B : A
{
}

interface IA {
    string Id {get;}
}

interface IB : IA {}

// class BImpl : IB
// {
//     public string Id => throw new NotImplementedException();
// }
