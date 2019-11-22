@annstart
### Ensemble voting problem
We are given $n$ classifiers and $c$ classes. Each classifier has one voice. Each classifier will vote for $class 0$
with the probability of $p$, $p > 1/c$. For each other class it will vote with the probability of $(1 - p) / (c - 1)$.
Once the classifiers have voted, we find out the most voted classes and pick one of them equally likely - the picked 
class called "elected." The problem is to find out the probability of $class 0$ being finally elected.
@annend

### Current investigation
Let us consider the case for $c=2$. For $n \mod 2 = 1$ we should consider all the cases when the number of
"correctly-voted" classifiers is at least $\frac{n - 1}{2} + 1$. The final formula will be
$$
\sum^{n}_{i=\frac{n - 1}{2} + 1} C^{n}_i p^i (1-p)^{n-i}
$$
For $n \mod 2 = 0$
$$
\sum^{n}_{i=\frac{n}{2} + 1} C^{n}_i p^i (1-p)^{n-i} + \frac{1}{2} C^{n}_{\frac{n}{2}} (p(1 - p))^{\frac{n}{2}}
$$

@probauthor{WhiteBlackGoose}
@investauthor{WhiteBlackGoose}