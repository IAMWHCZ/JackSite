import { Node, Edge } from 'reactflow';

interface Column {
  name: string;
  type: string;
  isPrimary: boolean;
  isForeign: boolean;
  references?: string;
  isNullable: boolean;
  defaultValue?: string;
  comment?: string;
  isUnique: boolean;
}

interface ParseResult {
  nodes: Node[];
  edges: Edge[];
}

export function parseSqlToGraph(sql: string): ParseResult {
  const tableRegex = /CREATE\s+TABLE\s+`?(\w+)`?\s*\(([\s\S]*?)\);/gim;
  // 更新正则表达式以支持中文列名
  const columnRegex = /`?([^`\s]+)`?\s+([\w()]+)(?:\s+PRIMARY\s+KEY)?(?:\s+REFERENCES\s+`?(\w+)`?(?:\s*\([^)]+\))?)?(?:\s+NOT\s+NULL)?(?:\s+UNIQUE)?(?:\s+DEFAULT\s+([^,\s]+))?(?:\s+COMMENT\s+['"]([^'"]+)['"])?/gim;

  const nodes: Node[] = [];
  const edges: Edge[] = [];
  let tableCount = 0;

  let match;
  while ((match = tableRegex.exec(sql)) !== null) {
    const tableName = match[1];
    const columnsStr = match[2];

    const columns: Column[] = [];

    let columnMatch;
    while ((columnMatch = columnRegex.exec(columnsStr)) !== null) {
      const [fullMatch, name, type, references, defaultValue, comment] = columnMatch;

      const column = {
        name,
        type,
        isPrimary: /PRIMARY\s+KEY/i.test(fullMatch),
        isForeign: !!references,
        references,
        isNullable: !/NOT\s+NULL/i.test(fullMatch),
        isUnique: /UNIQUE/i.test(fullMatch),
        defaultValue: defaultValue || undefined,
        comment: comment || undefined
      };

      columns.push(column);

      if (column.isForeign && column.references) {
        edges.push({
          id: `${tableName}-${column.references}-${column.name}`,
          source: tableName,
          target: column.references,
          animated: true,
          style: { stroke: '#2563eb' },
          label: `${column.name} → ${column.references}`,
        });
      }
    }

    nodes.push({
      id: tableName,
      type: 'tableNode',
      position: { x: 250 * tableCount, y: 200 * tableCount },
      data: { label: tableName, columns },
      draggable: true,
    });

    tableCount++;
  }

  return { nodes, edges };
}
