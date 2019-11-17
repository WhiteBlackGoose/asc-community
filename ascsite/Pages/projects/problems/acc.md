@annstart

### Two-bodies collision

<br>

When simulating the gravity force between two particles, we usually use the iterative method. 
However, it would be more efficiently to find the position of each of the two bodies at every moment of time. 
We are given two particles with one fixed and one floating, with masses $m_1$ and $m_2$ and the only 
force applied to the first one is $F = \frac{m_1 m_2}{(S - r)^2}$, where $S$ - the initial distance 
between the particles, $r$ - the current distance traveled by the first particle. We also know 
$v_1 = v_2 = 0$, $v_2 = const$. The goal is to find out the speed of the first particle at the moment 
of their collision.<br>
@annend

### Current investigation
Let us systemize our current knowledge.
$$
\begin{cases}
v_1=v_2=0 \\
a(r)=\frac{m_2}{(S-r)^2}\\ 
r(t)=\int v(t) dt=\int \int a(t) dt^2\\
r(0)=v(0)=0
\end{cases}
$$
As we see, $a(t)=\frac{m_2}{(S-r(t))^2}$ and $a(t) = \frac{d^2[r(t)]}{dt^2}$; hence, 
$$
\frac{d^2[r(t)]}{dt^2} = \frac{m_2}{(S-r(t))^2}
$$
$$
(S-r(t))^2 d^2[r(t)] = m_2 dt^2
$$
$$
\int \int (S-r)^2 dr^2 = \int \int m_2 dt^2
$$
$$
\int \int [S^2 - 2Sr + r^2] dr^2 = \int [m_2 t + c_1] dt
$$
$$
\int [S^2r - Sr^2 + \frac{r^3}{3} + c_2] dr = \int [m_2 t + c_1] dt
$$
$$
\frac{S^2r^2}{2} - \frac{Sr^3}{3} + \frac{r^4}{12} + c_2r + c_3 = \frac{m_2 t^2}{2} + c_1t + c_4
$$
Because the traveled distance is zero for $t=0$, $0 + c_3 = 0 + c_4$ meaning that $c_3 = c_4$ and we 
can eliminate them. We will also multiply the equation by 12 for convenience sake.
$$
r^4 - 4Sr^3 + 6S^2r^2 + 12c_2r - (6m_2 t^2 + 12c_1t) = 0
$$
Here we can somehow find out $r(t)$ as well as $t(r)$. However, by this moment we still do not know 
the constants $c_1$ and $c_2$ that can be probably found from the following fact
$$
v(0) = 0,  v(t) = \frac{d[r(t)]}{dt}
$$
Once we find out all the constants and $v(t)$ we should solve the equation above over $t$ for $r = S$ and
substitute $v(t_0)$ which is the answer to the problem.
<br>
<br>

By @WhiteBlackGoose