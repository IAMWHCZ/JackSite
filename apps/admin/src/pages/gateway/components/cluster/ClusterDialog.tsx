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
import type { ClusterConfig } from '@/types/gateway';
import { useGatewayStore } from '@/stores/gateway';

const defaultClusterConfig: ClusterConfig = {
  clusterId: '',
  loadBalancingPolicy: 'RoundRobin',
  sessionAffinity: {
    enabled: false,
    policy: 'Cookie',
    affinityKeyName: 'SessionAffinity',
    failurePolicy: 'Return503Error',
    cookie: {
      path: '/',
      httpOnly: true,
      secure: true,
      sameSite: 'Lax'
    }
  },
  healthCheck: {
    active: {
      enabled: true,
      interval: '00:00:10',
      timeout: '00:00:10',
      policy: 'ConsecutiveFailures',
      path: '/health'
    },
    passive: {
      enabled: false,
      policy: 'TransportFailureRate',
      reactivationPeriod: '00:00:10'
    }
  },
  httpClient: {
    dangerousAcceptAnyServerCertificate: false,
    maxConnectionsPerServer: 1024,
    enableMultipleHttp2Connections: false,
    requestHeaderEncoding: 'utf-8',
    responseHeaderEncoding: 'utf-8'
  },
  httpRequest: {
    version: '2.0',
    versionPolicy: 'RequestVersionOrLower',
    activityTimeout: '00:00:30',
    allowResponseBuffering: true
  },
  destinations: {},
  metadata: {}
}
const initialData = null;

export const ClusterDialog = () => {
  const { t } = useTranslation();
  const {openClusterDialog,setOpenClusterDialog} = useGatewayStore();

  const onClose = () => setOpenClusterDialog(false)
  // 处理关闭事件
  const handleClose = (_: {}, reason: "backdropClick" | "escapeKeyDown") => {
    // 只有当不是点击蒙层时才关闭
    if (reason !== "backdropClick") {
      onClose();
    }
  };

  const form = useForm({
    defaultValues:defaultClusterConfig,
    onSubmit: ({ value }) => {
      console.log(value);
    },
  });

  useEffect(() => {
    if (initialData) {
      form.reset();
    }
  }, [initialData, form]);

  return (
    <Dialog
      open={openClusterDialog}
      onClose={handleClose}
      maxWidth="lg"
      fullWidth
      PaperProps={{
        sx: { maxWidth: '1000px' }
      }}
    >
      <DialogTitle sx={{ m: 0, p: 2, pr: 6 }}>
        {initialData
          ? t('gateway.dialog.title.editCluster')
          : t('gateway.dialog.title.addCluster')
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
                name="clusterId"
                validators={{
                  onChange: ({ value }) =>
                    !value ? t('gateway.dialog.form.clusterId.error') : undefined,
                }}
              >
                {(field) => (
                  <TextField
                    disabled={!!initialData}
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.clusterId.label')}
                    fullWidth
                    required
                    error={!!field.state.meta.errors}
                    helperText={field.state.meta.errors}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="loadBalancingPolicy">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value as ClusterConfig['loadBalancingPolicy'])}
                    label={t('gateway.dialog.form.loadBalancing')}
                    fullWidth
                  >
                    {['PowerOfTwoChoices', 'FirstAlphabetical', 'Random', 'RoundRobin', 'LeastRequests'].map(item => (
                      <MenuItem key={item} value={item}>{item}</MenuItem>
                    ))}
                  </TextField>
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
            <Grid size={6}>
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
              <form.Field name="sessionAffinity.failurePolicy">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value as 'Redistribute' | 'Return503Error')}
                    label={t('gateway.dialog.form.sa.failurePolicy')}
                    fullWidth
                    disabled={!form.getFieldValue('sessionAffinity.enabled')}
                  >
                    <MenuItem value="Redistribute">Redistribute</MenuItem>
                    <MenuItem value="Return503Error">Return 503 Error</MenuItem>
                  </TextField>
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* 健康检查 */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.healthCheck')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            {/* Active Health Check */}
            <Grid size={6}>
              <form.Field name="healthCheck.active.enabled">
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
              <form.Field name="healthCheck.active.path">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.healthCheck.active.path')}
                    fullWidth
                    disabled={!form.getFieldValue('healthCheck.active.enabled')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="healthCheck.active.interval">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.healthCheck.active.interval')}
                    fullWidth
                    disabled={!form.getFieldValue('healthCheck.active.enabled')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="healthCheck.active.timeout">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.healthCheck.active.timeout')}
                    fullWidth
                    disabled={!form.getFieldValue('healthCheck.active.enabled')}
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="healthCheck.active.policy">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.healthCheck.active.policy')}
                    fullWidth
                    disabled={!form.getFieldValue('healthCheck.active.enabled')}
                  >
                    <MenuItem value="ConsecutiveFailures">Consecutive Failures</MenuItem>
                    <MenuItem value="ConsecutiveSuccess">Consecutive Success</MenuItem>
                  </TextField>
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* HTTP Client */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.httpClient')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
          <Grid size={6}>
              <form.Field name="httpClient.dangerousAcceptAnyServerCertificate">
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
              <form.Field name="httpClient.maxConnectionsPerServer">
                {(field) => (
                  <TextField
                    type="number"
                    value={field.state.value}
                    onChange={(e) => field.handleChange(Number(e.target.value))}
                    label={t('gateway.dialog.form.httpClient.maxConnections')}
                    fullWidth
                  />
                )}
              </form.Field>
            </Grid>
            <Grid></Grid>
            <Grid size={6}>
              <form.Field name="httpClient.enableMultipleHttp2Connections">
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
              <form.Field name="httpClient.requestHeaderEncoding">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.httpClient.requestHeaderEncoding')}
                    fullWidth
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="httpClient.responseHeaderEncoding">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.httpClient.responseHeaderEncoding')}
                    fullWidth
                  />
                )}
              </form.Field>
            </Grid>
          </Grid>

          <Divider sx={{ my: 3 }} />

          {/* HTTP Request */}
          <Typography variant="subtitle1" sx={{ mb: 2 }}>
            {t('gateway.dialog.section.httpRequest')}
          </Typography>
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid size={6}>
              <form.Field name="httpRequest.version">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.httpRequest.version')}
                    fullWidth
                  >
                    <MenuItem value="1.0">HTTP/1.0</MenuItem>
                    <MenuItem value="1.1">HTTP/1.1</MenuItem>
                    <MenuItem value="2.0">HTTP/2.0</MenuItem>
                  </TextField>
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="httpRequest.versionPolicy">
                {(field) => (
                  <TextField
                    select
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value as 'RequestVersionExact' | 'RequestVersionOrLower' | 'RequestVersionOrHigher')}
                    label={t('gateway.dialog.form.httpRequest.versionPolicy')}
                    fullWidth
                  >
                    <MenuItem value="RequestVersionExact">Request Version Exact</MenuItem>
                    <MenuItem value="RequestVersionOrLower">Request Version Or Lower</MenuItem>
                    <MenuItem value="RequestVersionOrHigher">Request Version Or Higher</MenuItem>
                  </TextField>
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="httpRequest.activityTimeout">
                {(field) => (
                  <TextField
                    value={field.state.value}
                    onChange={(e) => field.handleChange(e.target.value)}
                    label={t('gateway.dialog.form.httpRequest.activityTimeout')}
                    fullWidth
                  />
                )}
              </form.Field>
            </Grid>
            <Grid size={6}>
              <form.Field name="httpRequest.allowResponseBuffering">
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
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>{t('common.cancel')}</Button>
          <Button type="submit" variant="contained">{t('common.confirm')}</Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};



