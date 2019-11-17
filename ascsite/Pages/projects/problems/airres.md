@annstart
### Motion equation for a thrown particle and the air resistance
Consider the problem when the only force applied to the particle is the gravity force. For this problem
$x(t) = v_x t$ and $y(t) = v_y t - \frac{a t^2}{2}$ where $v_x = v cos(\phi)$ and $v_y = v sin(\phi)$ for 
$\phi$ - the angle between the horizone and the initial velocity. However, when it comes to extra force, 
the problem turns out to be more complicated.
@annend

We introduce the problem as
$$
\begin{cases}
\vec a(t) = \vec g - \vec v(t) |v(t)| k \\
\vec v(t) = \int a(t) dt + \vec v_0(t)
\end{cases}
$$

where $\vec g \approx (0; -9.81)$, $v_0(t)$ - the initial velocity. Our goal is to find out $x(t) = \int v_x(t) dt$ and $y(t) = \int v_y(t) dt$.

#### Current investigation
The system implies that
$$
\frac{d[\vec v(t)]}{dt} = \vec g - \vec v(t) |v(t)| k
$$
And, splitting into dimensions,
$$
\begin{cases}
\frac{d[v_x]}{dt} = -v_x\sqrt{v_x^2 + v_y^2} k \\
\frac{d[v_y]}{dy} = g - v_y\sqrt{v_x^2 + v_y^2} k \\
\end{cases}
$$