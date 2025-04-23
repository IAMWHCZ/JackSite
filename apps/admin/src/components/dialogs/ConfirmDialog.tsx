import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@mui/material';
import { useTranslation } from 'react-i18next';

export interface ConfirmDialogProps {
  open?: boolean;
  title?: string;
  content?: string;
  onConfirm: () => void;
  onCancel: () => void;
  confirmText?: string;
  cancelText?: string;
  confirmButtonProps?: React.ComponentProps<typeof Button>;
  cancelButtonProps?: React.ComponentProps<typeof Button>;
  disableBackdropClick?: boolean;
}

export const ConfirmDialog = ({
  open = false,
  title,
  content,
  onConfirm,
  onCancel,
  confirmText,
  cancelText,
  confirmButtonProps,
  cancelButtonProps,
  disableBackdropClick = false
}: ConfirmDialogProps) => {
  const { t } = useTranslation();

  const handleClose = (_?: {}, reason?: "backdropClick" | "escapeKeyDown") => {
    if (disableBackdropClick && reason === "backdropClick") {
      return;
    }
    onCancel();
  };

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      aria-labelledby="confirm-dialog-title"
      aria-describedby="confirm-dialog-description"
      maxWidth="xs"
      fullWidth
    >
      <DialogTitle id="confirm-dialog-title">
        {title}
      </DialogTitle>
      <DialogContent>
        <DialogContentText id="confirm-dialog-description">
          {content}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button 
          onClick={onCancel}
          color="inherit"
          {...cancelButtonProps}
        >
          {cancelText || t('common.cancel')}
        </Button>
        <Button 
          onClick={onConfirm}
          variant="contained"
          color="primary"
          autoFocus
          {...confirmButtonProps}
        >
          {confirmText || t('common.confirm')}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
