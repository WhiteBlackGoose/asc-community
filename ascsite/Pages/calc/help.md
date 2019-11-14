## ASC Calculator usage

<br>

### Syntax
Request consists of the following parts: field field ... field expression keywords

#### Field
Consists of field name and keywords.
Possible field names:
- derivative
- integrate
- solve
- boolean

#### Keywords
Are arguments of the request. Possible usage:
- keyword variable
- keyword variable = expression

Possible keywords:
- where
- for

For example, `where x=5` or `for z`.

#### Expression
Your request, for example `z^2 + 2*z - 3`. You can use `z2 + 2z - 3` as well.

### Examples
You can calculate
- 1 + 1
- log(23, 2) + sin(pi/3) / sqrt(23) + pi3

You can simplify
- x2 + y2x
- (2x + 2y) / (x + y)

You can derivate
- derivative x2 + log(x)
- derivative for z z*x - z/x

You can integrate
- integrate x2 - sin(x)

You can solve
- solve for y y2 + x*y - 3

You can evaluate boolean functions
- boolean a & b -> c

You can substitute
- (a2 + a4) / (z + z2) where z=sqrt(x + 3) where a=(2^x + sqrt(x))

You can process sequential requests
- derivative for x where c = 5 solve for z z2 - y2*x + t + c where t=sqrt(x)