## Original configuration

```puml
legend top left
| Pattern  | |
| <U+2014><U+2014><U+2014> | //Original// message        |
| ""- - -"" | Message //moved// by Bridge |
endlegend

package "SQL" {
  database "N1-database" as db1
  database "N2-database" as db2
  database "N3-database" as db3
  database "N4-database" as db4
}

component app1 [
  N1
]

component app2 [
  N2
]

component app3 [
  N3
]

component app4 [
  N4
]

[Bridge] as b

app1 <-[dotted]-> db1
app2 <-[dotted]-> db2
app3 <-[dotted]-> db3
app4 <-[dotted]-> db4

app1 -[#blue]-> b : " (1) ""CreateOrder"""
b -[#blue,dashed]-> app2 : " (1) ""CreateOrder"""

app2 -[#green]-> b : " (2) ""CreateOrderResponse"""
b -[#green,dashed]-> app1 : " (2) ""CreateOrderResponse"""

app2 -[#orange]-> b : " (3) ""OrderCreated"""
b -[#orange,dashed]-> app3 : " (3) ""OrderCreated"""
b -[#orange,dashed]-> app4 : " (3) ""OrderCreated"""
```

In this set up, everything works; anyway, I noticed that, when `CreateOrder` is moved to `N2-database`, bridge changes `ReplyToAddress` from `[N1]@[nsb]@[N1-datbase]` to `[N1]@[nsb]@[N3-database]`.

> I would have expected the value to be `[N1]@[nsb]@[N2-database]`
 
Because of this, when `N2` replies, i.e.

```csharp
await context.Reply(new CreateOrderResponse() {
  /* ... */    
});
```

this message is saved in table `N1` in `N3-database` (the account used by `N2` has full access to the server), and eventually, everything works: bridge picks up the message from `N1` queue in `N3-database`, and delivers it to `N1`, as expected.

Nonetheless, because of this unexpected (to me) behavior, I decided to move `N3-database` to another SQL Server instance.

## Updated configuration

```puml
package "SQL Server A" {
  database N1 as db1
  database N2 as db2
  database N4 as db4
}

package "SQL Server B" {
  database N3 as db3
}

component app1 [
  N1
]

component app2 [
  N2
]

component app3 [
  N3
]

component app4 [
  N4
]

[Bridge] as b

app1 <-[dotted]-> db1
app2 <-[dotted]-> db2
app3 <-[dotted]-> db3
app4 <-[dotted]-> db4

app1 -[#blue]-> b : " (1) ""CreateOrder"""
b -[#blue,dashed]-> app2 : " (1) ""CreateOrder"""

app2 -[#green]-> b : " (2) ""CreateOrderResponse"""
b -[#green,dashed]-> app1 : " (2) ""CreateOrderResponse"""

app2 -[#orange]-> b : " (3) ""OrderCreated"""
b -[#orange,dashed]-> app3 : " (3) ""OrderCreated"""
b -[#orange,dashed]-> app4 : " (3) ""OrderCreated"""
```

> Only change in the solution is the updated connection string, both in N3 and bridge configurations.   

Everything works as before, but this time the call to `Reply` fails, because `N2` cannot connect to `N3-database` now. 