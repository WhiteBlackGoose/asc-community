@annstart

### Two-bodies collision

<br>

When simulating the gravity force between two particles, we usually use the iterative method. However, it will be more efficiently to find position of each of two bodies at every moment of time. We are given two particles with one fixed and one floating, with masses $m_1$ and $m_2$ and the only force applied to the first one is $F = \frac{m_1 m_2}{(S - r)^2}$, where $S$ - the initial distance between the particles, $r$ - the current distance traveled by the first particle. We also know $v_1 = v_2 = 0$, $v_2 = const$. The goal is to find out the speed of the first particle at the moment of their collision.<br>
@annend

### Current investigation
Let us systemize our current knowledge.
$$
\begin{cases}
v_1=v_2=0 \\
a(r)=\frac{m_1 m_2}{(S-r)^2}\\ 
r(t)=\int v(t) dt=\int \int a(t) dt^2\\
r(0)=v(0)=0
\end{cases}
$$
As we see, $a(t)=\frac{m_1 m_2}{(S-r(t))}$

<br>
<br>

By @WhiteBlackGoose