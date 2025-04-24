import { CircularProgress, TableBody, TableRow, TableCell } from '@mui/material'
import { useTranslation } from 'react-i18next'

export const TableLoading = () => {
    const {t} = useTranslation()
    return (
        <TableBody sx={{height: 600}}>
            <TableRow>
                <TableCell colSpan={100} align="center" sx={{ py: 3 }}>
                    <CircularProgress size={100} sx={{ mr: 2 }} />
                    <div className='mt-4 text-2xl font-bold text-blue-500'>{t('loading.table')}</div>
                </TableCell>
            </TableRow>
        </TableBody>
    )
}
