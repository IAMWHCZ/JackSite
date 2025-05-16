import { TableDefinition } from '@/lib/sql-parser/type';
import { ERDiagram, EREdge, ERNode } from './type';

export function generateERDiagram(tables: TableDefinition[]): ERDiagram {
  const nodes: ERNode[] = [];
  const edges: EREdge[] = [];

  // 为每个表生成节点
  tables.forEach((table, index) => {
    const node: ERNode = {
      id: table.name,
      type: 'table',
      data: {
        label: table.name,
        columns: table.columns.map(col => ({
          name: col.name,
          type: `${col.dataType}${col.length ? `(${col.length})` : ''}`,
          isPrimary: col.primaryKey,
          isForeign: !!col.references
        }))
      },
      position: {
        x: (index % 3) * 300 + 50,
        y: Math.floor(index / 3) * 400 + 50
      }
    };
    nodes.push(node);
  });

  // 为外键关系生成边
  tables.forEach(table => {
    // 处理列级别的外键
    table.columns.forEach(column => {
      if (column.references) {
        edges.push({
          id: `${table.name}-${column.name}-${column.references.table}`,
          source: table.name,
          target: column.references.table,
          type: 'foreignKey',
          data: {
            sourceColumns: [column.name],
            targetColumns: [column.references.column]
          }
        });
      }
    });

    // 处理表级别的外键
    table.foreignKeys.forEach((fk, index) => {
      edges.push({
        id: `${table.name}-fk-${index}`,
        source: table.name,
        target: fk.referenceTable,
        type: 'foreignKey',
        data: {
          sourceColumns: fk.columns,
          targetColumns: fk.referenceColumns
        }
      });
    });
  });

  return { nodes, edges };
}
