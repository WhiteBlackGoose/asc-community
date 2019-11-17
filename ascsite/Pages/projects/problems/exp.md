@annstart

### Exponentials sum equation
Find invert function $ f(x) $ to solve $a^{x} + b^{x} = c$
@annend

#### Sub-Problem
Find invert function $ f(x) $ to solve $ a^{x} + b^{x} = c^{x} $.

This equation seems harder to solve, but actually it is special case of the original equation:
$ a^{x} + b^{x} = c^{x} \Leftrightarrow ({a \over c})^{x} + ({a \over c})^{x} = 1 $, where $ c = 1 $.

### Investigation
for $b = a$, this equation can be easily solved: $ a^{x} + a^{x} = c \Rightarrow a^x = {c \over 2} \Rightarrow x = log_{a}({c \over 2}) - 1 $.

In more general way, if we can find $ t $, such that $ b = a^{x t} $ the equation become polynomial after substitution $ y = a^{x} $ and can be solve using already existing methods: $y^t + y - c = 0$.
