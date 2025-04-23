import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  MenuItem,
  Grid,
  IconButton,
  Tooltip,
  Typography,
  Divider,
  Switch,
  FormControlLabel,
} from '@mui/material';
import { X } from 'lucide-react';
import { useForm } from '@tanstack/react-form';
import type { RouteConfig } from '@/types/gateway';

interface RouteDialogProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: RouteConfig) => void;
  initialData?: RouteConfig;
  clusters: string[];
}
const defaultRouteConfig: RouteConfig = {
  routeId: '',
  match: {
    path: '',
    hosts: [],
    methods: [],
    headers: [],
    queryParameters: []
  },
  order: undefined,
  clusterId: '',
  authorizationPolicy: 'Default',
  rateLimiterPolicy: '',
  outputCachePolicy: '',
  timeoutPolicy: '',
  timeout: '',
  corsPolicy: 'Default',
  maxRequestBodySize: undefined,
  metadata: {},
  transforms: [{
    pathPattern: '',
    pathRemovePrefix: '',
    requestHeadersCopy: true,
    requestHeadersRemove: [],
    responseHeadersCopy: true,
    responseHeadersRemove: []
  }],
  sessionAffinity: {
    enabled: false,
    policy: 'Cookie',
    affinityKeyName: '',
    failurePolicy: 'Return503Error',
    cookie: {
      path: '/',
      httpOnly: true,
      secure: true,
      sameSite: 'Lax'
    }
  }
}
export const RouteDialog = ({
  open,
  onClose,
  onSubmit,
  initialData,
  clusters,
}: RouteDialogProps) => {
  const { t } = useTranslation();

  // 处理关闭事件
  const handleClose = (_: {}, reason: "backdropClick" | "escapeKeyDown") => {
    // 只有当不是点击蒙层时才关闭
    if (reason !== "backdropClick") {
      onClose();
    }
  };

  const form = useForm({
    defaultValues: defaultRouteConfig,
    onSubmit: ({ value }) => {
      onSubmit(value);
    },
  });

  useEffect(() => {
    if (initialData) {
      form.reset(initialData);
    }
  }, [initialData, form]);

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      maxWidth="lg"
      fullWidth
      PaperProps={{
        sx: { maxWidth: '1000px' }
      }}
    >
      <DialogTitle sx={{ m: 0, p: 2, pr: 6 }}>
        {initialData
          ? t('gateway.dialog.title.edit')
          : t('gateway.dialog.title.add')
        }
        <Tooltip title={t('gateway.dialog.close')}>
          <IconButton
            onClick={onClose}
            sx={{
              position: 'absolute',
              right: 8,
              top: 8,
              color: (theme) => theme.palette.grey[500],
            }}
          >
            <X size={20} />
          </IconButton>
        </Tooltip>
      </DialogTitle>
      <form onSubmit={(e) => {
        e.preventDefault();
        e.stopPropagation();
        form.handleSubmit();
      }}>
        <DialogContent>
          {/* 基本信息 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.basic')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={6}>
              <form.Field
                name="routeId"
                validators={{
                  onChange: ({ value }) =>
                    !value ? t('gateway.dialog.form.routeId.error') : undefined,
                }}
              >
                {(field) => (
                  <TextField
                    disabled={!!initialData}
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.routeId.label')}
                    fullWidth
                    required
                    error={!!field.state.meta.errors}
                    helperText={field.state.meta.errors}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field
                name="match.path"
                validators={{
                  onChange: ({ value }) =>
                    !value ? t('gateway.dialog.form.matchPath.error') : undefined,
                }}
              >
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.matchPath.label')}
                    fullWidth
                    required
                    error={!!field.state.meta.errors}
                    helperText={field.state.meta.errors}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="order">
                {(field) => (
                  <TextField
                    type="number"
                    value={field.state.value ?? ''}
                    onChange={(e) => {
                      const value = e.target.value === '' ? undefined : Number(e.target.value);
                      field.handleChange(value);
                    }}
                    label={t('gateway.dialog.form.order.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.order.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field
                name="clusterId"
                validators={{
                  onChange: ({ value }) =>
                    !value ? t('gateway.dialog.form.targetCluster.error') : undefined,
                }}
              >
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.targetCluster.label')}
                    fullWidth
                    required
                    error={!!field.state.meta.errors}
                    helperText={field.state.meta.errors}
                  >
                    {clusters.map((cluster) => (
                      <MenuItem key={cluster} value={cluster}>
                        {cluster}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* 匹配规则 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.matching')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={12}>
              <form.Field name="match.methods">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value || []}
                    onChange={(e) => field.handleChange(() => e.target.value.split('\n').filter(Boolean))}
                    label={t('gateway.dialog.form.methods')}
                    fullWidth
                    SelectProps={{
                      multiple: true,
                    }}
                  >
                    {['GET', 'POST', 'PUT', 'DELETE', 'PATCH', 'HEAD', 'OPTIONS'].map((method) => (
                      <MenuItem key={method} value={method}>
                        {method}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              </form.Field>
            </Grid>
            <Grid size={12}>
              <form.Field name="match.hosts">
                {(field) => (
                  <TextField
                    value={field.state.value?.join('\n') || ''}
                    onChange={(e) => field.handleChange(e.target.value.split('\n').filter(Boolean))}
                    label={t('gateway.dialog.form.hosts.label')}
                    fullWidth
                    multiline
                    rows={3}
                    helperText={t('gateway.dialog.form.hosts.helper')}
                  />
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* 策略配置 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.policies')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={6}>
              <form.Field name="authorizationPolicy">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.authPolicy.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.authPolicy.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="rateLimiterPolicy">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.rateLimit.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.rateLimit.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="outputCachePolicy">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.cachePolicy.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.cachePolicy.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="timeout">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.timeout.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.timeout.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="corsPolicy">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.cors.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.cors.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="maxRequestBodySize">
                {(field) => (
                  <TextField
                    type="number"
                    value={field.state.value ?? ''}
                    onChange={(e) => {
                      const value = e.target.value === '' ? undefined : Number(e.target.value);
                      field.handleChange(value);
                    }}
                    label={t('gateway.dialog.form.maxBodySize.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.maxBodySize.helper')}
                  />
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* Session Affinity */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.sessionAffinity')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={12}>
              <form.Field name="sessionAffinity.enabled">
                {(field) => (
                  <FormControlLabel
                    control={
                      <Switch
                        checked={field.state.value}
                        onChange={(e) => field.handleChange(e.target.checked)}
                      />
                    }
                    label={t('common.enabled')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="sessionAffinity.policy">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value as 'Cookie' | 'CustomHeader')}
                    label={t('gateway.dialog.form.sa.policy')}
                    fullWidth
                    disabled={!form.getFieldValue('sessionAffinity.enabled')}
                  >
                    <MenuItem value="Cookie">Cookie</MenuItem>
                    <MenuItem value="CustomHeader">Custom Header</MenuItem>
                  </TextField>
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="sessionAffinity.affinityKeyName">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.sa.keyName')}
                    fullWidth
                    disabled={!form.getFieldValue('sessionAffinity.enabled')}
                  />
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* 转换配置 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.transforms')}
          </Typography>
          <Grid container spacing={2}>
            <Grid size={6}>
              <form.Field name="transforms[0].pathPattern">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.transformPattern.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.transformPattern.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="transforms[0].pathRemovePrefix">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.removePrefix.label')}
                    fullWidth
                    helperText={t('gateway.dialog.form.removePrefix.helper')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={12}>
              <form.Field name="transforms[0].requestHeadersCopy">
                {(field) => (
                  <FormControlLabel
                    control={
                      <Switch
                        checked={field.state.value}
                        onChange={(e) => field.handleChange(e.target.checked)}
                      />
                    }
                    label={t('gateway.dialog.form.transforms.copyRequestHeaders')}
                  />
                )}
              </form.Field>
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>
            {t('gateway.dialog.actions.cancel')}
          </Button>
          <Button type="submit" variant="contained">
            {t('gateway.dialog.actions.save')}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};











