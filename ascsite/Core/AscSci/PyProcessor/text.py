class BoolFool:
    def __init__(self, eq):
        self.operators = {}
        self.synonims = {}
        self.add_synonim("¬", "!")
        self.add_synonim("∧", "&")
        self.add_synonim("^", "&")
        self.add_synonim("∨", "|")
        self.add_synonim("≡", "==")
        self.add_synonim("not", "!")
        self.add_synonim("and", "&")
        self.add_synonim("or", "|")
        self.add_operator1f("!", "10")
        self.add_operator2f("&", "0001")
        self.add_operator2f("|", "0111")
        self.add_operator2f("->", "1101")
        self.add_operator2f("==", "1001")
        self.eq = self.__simplify__(eq)

    def __add_ops__(self, syntax, keys, values):
        self.operators[syntax] = {}
        for i, key in enumerate(keys):
            self.operators[syntax][key] = values[i]

    def __simplify__(self, eq):
        eq = eq.lower()
        for syn in self.synonims:
            eq = eq.replace(syn, self.synonims[syn])
        eq = eq.replace(" ", "")
        eq = eq.replace("(1)", "1")
        eq = eq.replace("(0)", "0")
        return eq

    def __plugin__(self, eq, variables):
        for vkey in variables:
            eq = eq.replace(vkey, variables[vkey])
        return eq

    def __compile_operator_s__(self, op, key):
        if len(key) == 1:
            return op + key
        else:
            return key[0] + op + key[1]

    def __compile_operator__(self, operator):
        res = {}
        for key in self.operators[operator]:
            res[self.__compile_operator_s__(operator, key)] = self.operators[operator][key]
        return res

    def __take_min_pos__(self, s, values):
        r = [s.find(v) for v in values]
        r = [(i, el) for i, el in enumerate(r) if el != -1]
        m = (-1, len(s))
        for el in r:
            if el[1] < m[1]:
                m = el
        return m[0]

    def __minarg__(self, arr):
        return arr.index(min(arr))

    def __exec__(self, s, operator):
        table = self.__compile_operator__(operator)
        keys = list(table.keys())
        pos = self.__take_min_pos__(s, keys)
        while pos != -1:
            s = s.replace(keys[pos], table[keys[pos]], 1)
            s = self.__simplify__(s)
            pos = self.__take_min_pos__(s, keys)
        return s

    def __reserved_tokens__(self):
        return "()" + "".join(list(self.operators.keys()))

    def __permutations__(self, l, values):
        if l == 1:
            return ["0", "1"]
        res = []
        for v in values:
            res.extend([v + i for i in self.__permutations__(l - 1, values)])
        return sorted(res)

    def __str__(self):
        tokens = self.get_tokens()
        res = "| " + " | ".join(tokens) + " | F |"
        res += "\n" + "-" * len(res)
        for variables, value in self.compile_table_iter():
            res += "\n| " + " | ".join(variables.values()) + " | " + value + " |"
        return "F = " + self.eq + "\n\n" + res

    def _ipython_display_(self):
        print(self)

    def add_synonim(self, key, value):
        self.synonims[key] = value

    def add_operator1f(self, syntax, values):
        keys = ["0", "1"]
        self.__add_ops__(syntax, keys, values)

    def add_operator2f(self, syntax, values):
        keys = ["00", "01", "10", "11"]
        self.__add_ops__(syntax, keys, values)

    def solve(self, **variables):
        for v in variables:
            variables[v] = str(variables[v])
        exp = self.__simplify__(self.eq)
        exp = self.__plugin__(exp, variables)
        oldexp = exp
        while len(exp) > 1:
            for op in self.operators.keys():
                exp = self.__exec__(exp, op)
            if exp == oldexp:
                return exp
            oldexp = exp
        return exp

    def get_tokens(self):
        eq = self.__simplify__(self.eq)
        reserved = self.__reserved_tokens__()
        res = []
        for i in eq:
            if i not in reserved:
                res.append(i)
        return sorted(list(set(res)))

    def compile_table_iter(self, val=None):
        tokens = self.get_tokens()
        for values in self.__permutations__(len(tokens), "01"):
            variables = {}
            for key, value in zip(tokens, values):
                variables[key] = value
            value = self.solve(**variables)
            if val is None or value == val:
                yield variables, value

bf = BoolFool("a -> b")
for i in bf.compile_table_iter():
    print(i)