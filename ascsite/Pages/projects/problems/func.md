@annstart
### Non-trivial function equation
Find all functions $f(x)$ that satisfy $f(x) = xf(x^2) + x$
@annend

#### Current investigation
Note that:

this function is odd: $f(-x) = -xf(x^2) - x = -(xf(x^2) + x) = -f(x)$

its value in $x = 0$ is: $f(0) = 0*f(0^2) + 0 = 0$.

If $|x| > 1$, the value of function diverges (as $x^2 > x \Rightarrow f(x^2) > f(x)$).

In point $x = 1$ the function is not defined, as $f(1) = f(1) + 1 \Rightarrow 0 = 1$.

The most interesting part happens when $ 0 < x < 1 $ as for such values $x^2 < x$ and so $ x f(x^2) + x > x^2 f(x^4) + x^2 $. The term $x f(x^2)$ produce members of diminishing progression as $ \lim \limits_{x \to 0} f(x) = 0 $. This progression can be expressed in form:

$$ f(x) = \sum \limits_{i = 1}^{\infty} x^{2^i - 1} $$. This sum can be computed numerically:

$ f({1 \over 2}) \approx 0.632843 $

$ f({1 \over 3}) \approx 0.370828 $

$ f({1 \over 5}) \approx 0.208013 $

$ \lim \limits_{x \to 1} f(x) = + \infty $