import os

from ruamel.yaml import YAML
from ruamel.yaml.comments import CommentedSeq, CommentedMap
from ruamel.yaml.scalarint import HexCapsInt

yaml = YAML()
with open(os.path.join("..", "..", "ida", "data.yml"), 'r') as f:
    data = yaml.load(f)

new_structs = {}

for cls, clsData in data['classes'].items():
    new_class = {}
    if clsData is None:
        new_structs[cls] = None
        continue
    if 'inherits_from' in clsData:
        new_class['inherits'] = inherits = CommentedSeq([clsData['inherits_from']])
        inherits.fa.set_flow_style()
    if 'vtbl' in clsData:
        new_class['vtbls'] = vtbls = CommentedSeq([HexCapsInt(clsData['vtbl'])])
        vtbls.fa.set_flow_style()
    if 'vfuncs' in clsData:
        new_class['vfuncs'] = clsData['vfuncs']
    if 'funcs' in clsData:
        new_funcs = CommentedMap()
        for offset in sorted(clsData['funcs'], key=lambda k: HexCapsInt(k)):
            new_funcs[offset] = clsData['funcs'][offset]
            if offset in clsData['funcs'].ca.items:
                new_funcs.yaml_add_eol_comment(clsData['funcs'].ca.items[offset][2].value, offset)
        new_class['funcs'] = new_funcs
    new_structs[cls] = new_class

with open(os.path.join("..", "classes.yml"), 'w') as f:
    yaml.dump(new_structs, f)
